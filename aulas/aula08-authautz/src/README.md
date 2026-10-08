# API Biblioteca — Operação Crachá

Projeto prático da **Aula 08 — AuthN & AuthZ (JWT, OAuth 2.0 e OpenID Connect)**.

Você recebe a **API de Biblioteca da Aula 02 funcionando** — CRUD completo de livros —
mas com uma porta escancarada: **qualquer pessoa pode criar, editar e apagar livros**.
Sua missão é colocar um crachá digital na porta.

## Como rodar

```bash
cd src
dotnet run
```

- API: `http://localhost:5290`
- Swagger: `http://localhost:5290/swagger`
- Acervo em memória (sem banco): reiniciar a API restaura os dados iniciais.

## Usuários fictícios

| E-mail | Senha | Role |
|---|---|---|
| `leitor@biblioteca.com` | `leitor123` | `Leitor` |
| `biblio@biblioteca.com` | `biblio123` | `Bibliotecario` |

## Endpoints

| Método | Rota | Hoje | Depois da prática |
|---|---|---|---|
| GET | `/api/livros` | aberto | continua aberto |
| GET | `/api/livros/{id}` | aberto | continua aberto |
| POST | `/api/livros` | **aberto** | exige token |
| PUT | `/api/livros/{id}` | **aberto** | exige token |
| DELETE | `/api/livros/{id}` | **aberto** | exige token **e** role `Bibliotecario` |
| POST | `/api/auth/login` | a implementar | emite o JWT |
| POST | `/api/auth/login-expirado` | pronto | emite token já vencido (para testar 401) |
| GET | `/api/quem-sou-eu` | pronto | mostra as claims do seu token |

## Roteiro da prática (75 min, em duplas)

Procure os comentários `TODO` numerados no código — eles guiam cada etapa.

**Etapa 1 — Configurar (Program.cs, ~15 min).**
Registrar `AddAuthentication` + `AddJwtBearer` com os parâmetros de validação, e ligar
`UseAuthentication()` / `UseAuthorization()` **nessa ordem**. Os pacotes já estão no
`.csproj` (num projeto novo você rodaria `dotnet add package
Microsoft.AspNetCore.Authentication.JwtBearer`).

**Etapa 2 — Emitir o token (AuthController.cs, ~15 min).**
Implementar o `POST /api/auth/login`: validar as credenciais e devolver o JWT. O método
`GerarToken` já vem pronto — **leia-o com atenção**, é ali que as claims viram o crachá.

**Etapa 3 — Proteger a escrita (LivrosController.cs, ~10 min).**
`[Authorize]` no POST e no PUT. Sem token: **401**.

**Etapa 4 — Exigir a role (LivrosController.cs, ~10 min).**
`[Authorize(Roles = "Bibliotecario")]` no DELETE. Com token de Leitor: **403**.

**Etapa 5 — Testar o ciclo no Postman (~25 min).**
Importe `BibliotecaApi.postman_collection.json` e execute na ordem. O resultado esperado:

| # | Requisição | Status |
|---|---|---|
| 1 | POST `/api/livros` sem token | **401** |
| 2 | Login como Leitor | 200 + token |
| 3 | POST `/api/livros` com token de Leitor | **201** |
| 4 | DELETE `/api/livros/1` com token de Leitor | **403** |
| 5 | Login como Bibliotecário | 200 + token |
| 6 | DELETE `/api/livros/1` com token de Bibliotecário | **204** |
| 7 | POST com token expirado | **401** |

**Entregável:** os 7 casos acima demonstrados no Postman.

## O ponto que a aula quer fixar

**401 ≠ 403.**
`401 Unauthorized` significa "não sei quem você é" — falta token, ou ele é inválido/expirado.
`403 Forbidden` significa "sei exatamente quem você é, e mesmo assim você não pode".
O primeiro é sobre **autenticação**; o segundo, sobre **autorização**.

E lembre-se: o payload do JWT **não é criptografado**. Cole seu token em
[jwt.io](https://jwt.io) e leia o conteúdo — qualquer um consegue. A assinatura não esconde,
ela **impede a adulteração**. Por isso nunca coloque senha ou dado sensível no payload.

O gabarito completo está em `GABARITO.txt` — consulte só depois de tentar.
