# API Vazada — Operação Blindagem

Projeto prático da **Aula 07 — Segurança em Aplicações Web (OWASP Top 10:2025)**.

Esta é uma API .NET **propositalmente vulnerável**, usada como alvo didático. Ela contém
**exatamente 4 falhas**, as mesmas do handout impresso da aula. Cada uma está marcada no
código com um comentário `// FALHA N (não corrija ainda!)`.

> ⚠️ **Uso restrito à sala de aula.** Roda apenas em `localhost`, com dados fictícios de uma
> loja de brinquedo. Não publique esta API em nenhum servidor acessível pela internet.

## Como rodar

```bash
cd src
dotnet run
```

- API: `http://localhost:5275`
- Swagger: `http://localhost:5275/swagger`
- O banco SQLite (`apivazada.db`) é criado e populado automaticamente na primeira execução.
  Para começar do zero, apague o arquivo e rode de novo.

## Usuários de teste

| E-mail | Senha | Id | Role |
|---|---|---|---|
| `ana@loja.com` | `admin123` | 1 | Admin |
| `joao@loja.com` | `joao123` | 42 | User |
| `maria@loja.com` | `maria123` | 43 | User |

Pegue um token em `POST /api/login` e use no Postman como `Authorization: Bearer <token>`.

## As 4 falhas

| # | Falha | Onde está | Como demonstrar |
|---|---|---|---|
| 1 | Injeção de SQL | `Controllers/ProdutosController.cs` → `Buscar` | `GET /api/produtos/buscar?nome=%' OR '1'='1` retorna tudo |
| 2 | IDOR (Broken Access Control) | `Controllers/PedidosController.cs` → `GetById` | logado como João (42), `GET /api/pedidos/103` mostra o pedido da Maria (43) |
| 3 | Segredos no código + erro exposto | `Program.cs` | chave JWT e connection string em constantes; `GET /api/produtos/quebrar` devolve stack trace |
| 4 | Mass assignment | `Controllers/UsuariosController.cs` → `Atualizar` | `PUT /api/usuarios/42` com `"role": "Admin"` promove o próprio usuário |

## Roteiro da prática (75 min, em duplas)

1. **Recon (10 min).** Suba a API, abra o Swagger e reproduza as 4 falhas conforme a tabela
   acima. Anote, para cada uma, **qual dano um atacante causaria**.
2. **Blindagem (50 min).** Corrija **uma falha por vez**, na ordem 1 → 4. Depois de cada
   correção, repita o teste que a explorava e confirme que a resposta mudou:
   - Falha 1: a busca com `' OR '1'='1` deve deixar de listar tudo.
   - Falha 2: João pedindo o pedido da Maria deve receber **403 Forbidden**.
   - Falha 3: o erro deve virar uma mensagem genérica, sem stack trace; nenhum segredo no código.
   - Falha 4: enviar `"role": "Admin"` deve ser **ignorado** — a role permanece `User`.
3. **Verificação cruzada (15 min).** Troque de máquina com outra dupla e tente furar as
   correções dela.

**Entregável:** as 4 correções demonstradas no Postman, com print do antes e do depois.

## Dicas por falha

- **1:** use parâmetros (`FromSqlRaw` com `{0}` ou `FromSqlInterpolated`) ou simplesmente LINQ.
- **2:** compare o dono do recurso com a claim `NameIdentifier` do token e devolva `Forbid()`.
- **3:** mova segredos para configuração (`appsettings` / `dotnet user-secrets`) e condicione a
  página de exceção detalhada a `app.Environment.IsDevelopment()`.
- **4:** receba um **DTO de entrada** que só contenha os campos editáveis pelo cliente.

O gabarito completo está em `GABARITO.txt` — consulte só depois de tentar.
