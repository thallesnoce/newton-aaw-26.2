# LojaRevisao — Operação Pente-Fino

Projeto prático da **Aula 11.1 — Revisão das Aulas 01 a 11**.

A LojaRevisao vai ao ar na sexta-feira, e a auditoria achou problemas. A API compila e roda, mas
tem **11 falhas, uma de cada aula**. O número da falha é o número da aula: a `FALHA 07` é da
Aula 07. Cada uma está marcada no código com um comentário
`FALHA NN · AULA NN — tema (não corrija ainda!)`, que traz o sintoma e a sua tarefa.

> ⚠️ **Uso restrito à sala de aula.** A API é propositalmente vulnerável. Rode apenas em `localhost`.

## Como rodar

```bash
cd src
dotnet run --project LojaRevisao.Api
```

- API: `http://localhost:5311`
- Swagger: `http://localhost:5311/swagger`
- O banco SQLite é recriado a cada execução: **reiniciar a API restaura os dados**.

## Usuários de teste

Pegue o token em `POST /api/v1/auth/login` e use como `Authorization: Bearer <token>`.

| Id | Nome | E-mail | Senha | Papel |
|---|---|---|---|---|
| 1 | Ana | `ana@loja.com` | `admin123` | `Admin` |
| 42 | João | `joao@loja.com` | `joao123` | `Cliente` |
| 43 | Maria | `maria@loja.com` | `maria123` | `Cliente` |

Dados iniciais: o produto 3 custa exatamente R$ 300,00; o pedido **101** é do João e o **103** é da Maria.

## As 11 falhas

| Falha | Aula | Tema | Onde está | Sintoma |
|---|---|---|---|---|
| 01 | 01 | Fronteira entre módulos | `Modulos/Pedidos/PedidoService.cs` | pedido de 999.999 unidades deixa o estoque negativo |
| 02 | 02 | Status code | `Controllers/ProdutosController.cs` | produto inexistente responde 200; criação responde 200 |
| 03 | 03 | Design da URI | `Controllers/PedidosController.cs` | `GET /api/v1/cancelarPedido?id=N` cancela o pedido |
| 04 | 04 | Paginação | `Controllers/ProdutosController.cs` | a listagem devolve os 2.000 produtos, sempre |
| 05 | 05 | Atomicidade (ACID) | `Modulos/Pedidos/PedidoService.cs` | pedido que falha no 2º item já baixou o estoque do 1º |
| 06 | 06 | Comunicação assíncrona | `Modulos/Pedidos/PedidoService.cs` | cada pedido demora mais de 3 s esperando o e-mail |
| 07 | 07 | Injeção de SQL | `Controllers/ProdutosController.cs` | a busca com `%' OR '1'='1` ignora o filtro |
| 08 | 08 | Token expirado | `Program.cs` | token vencido continua abrindo a API |
| 09 | 09 | RBAC e ABAC | `ProdutosController.cs` e `PedidosController.cs` | Cliente remove produto; João abre o pedido da Maria |
| 10 | 10 | Contrato OpenAPI | `PedidosController.cs` e `Program.cs` | o Swagger anuncia 200, omite erros e não tem Authorize |
| 11 | 11 | Testes e bordas | `LojaRevisao.Tests` e `CalculadoraDePedido.cs` | compra de R$ 300,00 paga frete, com a suíte toda verde |

## Roteiro da prática (75 min, em duplas)

1. **Recon (10 min).** Suba a API, importe `LojaRevisao.postman_collection.json` no Postman e rode a
   coleção inteira (*Run collection*). Cada pasta é uma falha, e os testes afirmam o resultado
   **correto**: vermelho é falha ainda aberta. Todas as 11 pastas começam com algum teste vermelho.
2. **Correção (55 min).** Corrija **uma falha por vez**, na ordem 01 → 11. Para cada uma:
   1. procure `FALHA NN` no código (`Ctrl+Shift+F` no VSCode) e leia o comentário;
   2. faça a correção, lembrando do que foi visto na aula indicada;
   3. pare a API (`Ctrl+C`), suba de novo e rode a pasta da falha;
   4. anote o resultado no **diário de correção** da atividade impressa.
3. **Verificação cruzada (10 min).** Troque de máquina com outra dupla e rode a coleção inteira na
   correção dela. Os testes ficaram verdes pelo motivo certo?

A Falha 11 tem um passo a mais, com a API **parada** (com ela no ar, o build não consegue
sobrescrever os arquivos):

```bash
dotnet test
```

Antes de mexer em qualquer coisa, a suíte passa inteira, mesmo com o bug. Esse é o problema.

**Entregável:** coleção inteira verde (print do Runner), `dotnet test` verde e o diário de correção
preenchido.

## Dicas por falha

Tente primeiro sem elas. Se a dica não bastar, volte ao material da aula indicada.

- **01:** o que `ICatalogo` oferece que o `PedidoService` não está usando?
- **02:** `NotFound()` e `CreatedAtAction(...)`.
- **03:** `[HttpDelete("{id:int}")]` e `NoContent()`. Repetir o DELETE deve responder 404.
- **04:** `Skip`, `Take`, `OrderBy` e `Math.Clamp`; responda com `new Pagina<Produto>(...)`.
- **05:** quantos `SaveChanges` um pedido precisa? Lembre-se de que o EF Core grava tudo o que está
  pendente numa única transação.
- **06:** o serviço já recebe a `FilaDeNotificacoes`. O e-mail chega alguns segundos depois: confira
  em `GET /api/v1/notificacoes` (token da Ana).
- **07:** `FromSqlRaw` aceita parâmetros com `{0}`. Dá também para não escrever SQL.
- **08:** procure em `TokenValidationParameters`.
- **09:** parte A: `[Authorize(Roles = "...")]`. Parte B: claim `NameIdentifier`, `User.IsInRole` e
  `Forbid()`. O teste de cancelar pedido alheio só fica verde com as Falhas 03 e 09 corrigidas.
- **10:** `[ProducesResponseType]`; `AddSecurityDefinition` e `AddSecurityRequirement`. Na Aula 10
  está o passo a passo. Precisa de `using Microsoft.OpenApi.Models;`.
- **11:** o esperado do teste vem das regras escritas em `CalculadoraDePedido.cs`, nunca do código.

## Estrutura

```
src/
├── LojaRevisao.sln
├── LojaRevisao.postman_collection.json   ← os testes das 11 falhas
├── LojaRevisao.Api/
│   ├── Controllers/                      ← Auth, Produtos, Pedidos, Notificacoes
│   ├── Data/                             ← EF Core + SQLite e dados iniciais
│   ├── Models/
│   └── Modulos/
│       ├── Catalogo/                     ← regras de estoque, atrás de ICatalogo
│       ├── Pedidos/                      ← PedidoService e CalculadoraDePedido
│       └── Notificacoes/                 ← e-mail lento, fila e consumidor prontos
├── LojaRevisao.Tests/                    ← xUnit (Falha 11)
└── GABARITO.txt                          ← uso do professor
```

O gabarito completo está em `GABARITO.txt`. Consulte só depois de tentar.
