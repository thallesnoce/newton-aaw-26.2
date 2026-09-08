# Aula 06 — Comunicação entre Serviços — Prática

**Síncrono vs. Assíncrono na prática: sinta a diferença no Postman.**

## Cenário: LojaDev

Uma loja online simplificada com 3 serviços:

| Serviço | Porta | O que faz |
|---------|-------|-----------|
| **LojaApi** | 5090 | Recebe pedidos do cliente (Postman) |
| **PagamentoApi** | 5091 | Aprova/rejeita pagamentos (~1s de latência) |
| **NotificacaoApi** | 5092 | Envia e-mail (~3s de latência, ~20% de falha) |

O serviço de notificação é **propositalmente lento e instável** — é isso que justifica usar comunicação assíncrona.

## Conteúdo desta pasta

| Item | O que é |
|------|---------|
| `LojaDev.slnx` | Solution com os 3 projetos |
| `LojaDev.Compartilhado/` | Modelos compartilhados + fila em memória (`Channel<T>`) |
| `LojaDev.PagamentoApi/` | Serviço de pagamento — **PRONTO** (não alterar) |
| `LojaDev.NotificacaoApi/` | Serviço de notificação — **PRONTO** (não alterar) |
| `LojaDev.LojaApi/` | Serviço principal — **AQUI estão os TODOs** |
| `GABARITO/` | Gabarito dos TODOs (uso do professor) |

## Pré-requisitos

- .NET 8 SDK (ou superior)
- Postman (ou similar)
- 3 terminais abertos (um por serviço)

---

## Roteiro da prática (em duplas — 75 min)

### FASE 1 — Explorar os serviços auxiliares (10 min)

Abra **3 terminais** e suba os serviços auxiliares:

**Terminal 1** — PagamentoApi:
```bash
cd LojaDev.PagamentoApi
dotnet run
```

**Terminal 2** — NotificacaoApi:
```bash
cd LojaDev.NotificacaoApi
dotnet run
```

No **Postman**, teste cada serviço individualmente:

#### Teste 1: PagamentoApi
```
POST http://localhost:5091/api/pagamentos
Content-Type: application/json

{
  "cliente": "Maria Silva",
  "produto": "Notebook Dell",
  "valor": 4500.00
}
```
→ Deve retornar **aprovado = true** em ~1 segundo.

#### Teste 2: PagamentoApi (rejeição)
```
POST http://localhost:5091/api/pagamentos
Content-Type: application/json

{
  "cliente": "João Santos",
  "produto": "Carro Elétrico",
  "valor": 15000.00
}
```
→ Deve retornar **aprovado = false** (valor > R$ 10.000).

#### Teste 3: NotificacaoApi
```
POST http://localhost:5092/api/notificacoes
Content-Type: application/json

{
  "destinatario": "maria@email.com",
  "assunto": "Teste",
  "corpo": "Olá mundo"
}
```
→ Leva **~3 segundos**. Pode **falhar** (~20% das vezes). Repita se falhar.

📝 **Anote**: quanto tempo cada serviço leva para responder?

---

### FASE 2 — Implementar o fluxo SÍNCRONO (20 min)

**Terminal 3** — LojaApi:
```bash
cd LojaDev.LojaApi
dotnet run
```

Abra `LojaDev.LojaApi/Controllers/PedidosController.cs` e complete:

- **TODO 1** — Chamar PagamentoApi via HTTP
- **TODO 2** — Chamar NotificacaoApi via HTTP

Após completar, reinicie a LojaApi (`Ctrl+C` e `dotnet run` de novo).

No Postman, teste o fluxo síncrono:

```
POST http://localhost:5090/api/pedidos/sincrono
Content-Type: application/json

{
  "cliente": "Maria Silva",
  "produto": "Notebook Dell",
  "valor": 4500.00
}
```

📝 **Anote o `tempoTotalMs`** — deve ser **~4-5 segundos** (1s pagamento + 3s notificação).

🔁 Repita 3 vezes. Alguma falhou? O que aconteceu com a resposta quando a notificação falha?

---

### FASE 3 — Implementar o fluxo ASSÍNCRONO (25 min)

Agora complete os TODOs restantes:

Em `Controllers/PedidosController.cs`:
- **TODO 3** — Chamar PagamentoApi (copie do TODO 1)
- **TODO 4** — Publicar evento na fila
- **TODO 5** — Retornar `Accepted()` (HTTP 202)

Em `Servicos/NotificacaoBackground.cs`:
- **TODO 6** — Consumir eventos da fila com `await foreach`
- **TODO 7** — Chamar NotificacaoApi via HTTP (com try/catch)

Reinicie a LojaApi e teste o fluxo assíncrono no Postman:

```
POST http://localhost:5090/api/pedidos/assincrono
Content-Type: application/json

{
  "cliente": "Maria Silva",
  "produto": "Notebook Dell",
  "valor": 4500.00
}
```

📝 **Anote o `tempoTotalMs`** — deve ser **~1 segundo** (só pagamento!).

👀 **Olhe o terminal da LojaApi**: a notificação é processada em background, DEPOIS que a resposta já foi pro Postman.

---

### FASE 4 — Experimentação e análise (15 min)

#### Experimento 1: Rajada de pedidos
No Postman, envie **5 pedidos assíncronos** em sequência rápida. Observe:
- Os 5 retornam rápido (~1s cada)?
- O background processa os 5 na sequência? (olhe os logs)
- Alguma notificação falhou? O que aconteceu? (a resposta do pedido mudou?)

#### Experimento 2: Notificação fora do ar
Pare o NotificacaoApi (`Ctrl+C` no Terminal 2). Envie um pedido:
- **Síncrono** (`/api/pedidos/sincrono`) → O que acontece?
- **Assíncrono** (`/api/pedidos/assincrono`) → O que acontece?

Suba o NotificacaoApi de novo e observe os logs.

#### Experimento 3: Pagamento rejeitado
Envie um pedido com valor > R$ 10.000 em ambos os fluxos:
```json
{
  "cliente": "João Santos",
  "produto": "Carro Elétrico",
  "valor": 15000.00
}
```
- O pedido rejeitado gera notificação? Por quê?

---

### FASE 5 — Reflexão (5 min)

Responda no caderno ou em um comentário no código:

1. **Por que o pagamento é síncrono nos dois fluxos?** Poderia ser assíncrono?

2. **O que acontece se a fila perder um evento?** (Lembre: o `Channel<T>` está em memória — se a aplicação reiniciar, o que acontece com os eventos na fila?)

3. **Em produção, o que substituiria o `Channel<T>`?** Cite pelo menos uma tecnologia.

4. **Se a NotificacaoApi falhar, como o consumidor da fila deveria reagir?** (Dica: o gabarito tem a resposta parcial, mas pesquise sobre "dead-letter queue" e "retry with backoff").

5. **Compare os tempos de resposta** que você anotou:

| Fluxo | Tempo de resposta | Notificação chega? |
|-------|-------------------|--------------------|
| Síncrono | ~_____ ms | ☐ Imediata / ☐ Pode falhar |
| Assíncrono | ~_____ ms | ☐ Em background / ☐ Garantida? |

---

## Entregável

- API funcionando nos dois fluxos (prints do Postman: síncrono e assíncrono, com os tempos)
- Tabela comparativa preenchida (Fase 5, pergunta 5)
- Respostas das perguntas 1-4

## Dica importante

Olhe os **logs nos terminais** — eles contam a história completa do que está acontecendo em cada serviço. No fluxo assíncrono, preste atenção na ORDEM dos logs: a resposta do Postman aparece ANTES da notificação ser processada!

## Gabarito

`GABARITO/PedidosController.Gabarito.cs.txt` e `GABARITO/NotificacaoBackground.Gabarito.cs.txt` — versões completas com todos os TODOs resolvidos (professor: não distribuir antes).
