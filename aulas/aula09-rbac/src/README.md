# API Biblioteca — Operação Crachá, Nível 2

Projeto prático da **Aula 09 — RBAC, ABAC e PBAC (autorização)**.

Você recebe a API de Biblioteca **como terminou a Aula 08**: o login emite JWT e a escrita
já exige token. A autenticação (401) está resolvida. O problema agora é **autorização** (403):
a API sabe quem você é, mas decide errado o que você pode fazer.

O código tem **exatamente 4 falhas de autorização**, cada uma marcada com um comentário
`FALHA N (não corrija ainda!)`. Compila e roda; só decide errado.

## Como rodar

```bash
cd src
dotnet run
```

- API: `http://localhost:5291`
- Swagger: `http://localhost:5291/swagger`
- Dados em memória (sem banco): reiniciar a API restaura os dados iniciais.

## Usuários fictícios

| Id | Nome | E-mail | Senha | Papel |
|---|---|---|---|---|
| 1 | Ana | `ana@biblioteca.com` | `ana123` | `Leitor` |
| 2 | Bruno | `bruno@biblioteca.com` | `bruno123` | `Leitor` |
| 3 | Carla | `carla@biblioteca.com` | `carla123` | `Pesquisador` |
| 4 | Beto | `biblio@biblioteca.com` | `biblio123` | `Bibliotecario` |
| 5 | Dora | `admin@biblioteca.com` | `admin123` | `Admin` |

Dados iniciais: livros 1–3 são `Comum`, o livro 4 (Os Lusíadas, 1572) é `ObraRara`.
Empréstimo **101** é da Ana; empréstimo **102** é do Bruno.

## A matriz de acesso (a especificação que a API deveria cumprir)

| Ação | Endpoint | Leitor | Pesquisador | Bibliotecario | Admin | Modelo |
|---|---|---|---|---|---|---|
| Consultar acervo | `GET /api/livros` | ✓ (até sem login) | ✓ | ✓ | ✓ | público |
| Cadastrar / editar livro | `POST`, `PUT /api/livros` | ✗ | ✗ | ✓ | ✓ | RBAC |
| Remover livro | `DELETE /api/livros/{id}` | ✗ | ✗ | ✗ | ✓ | RBAC |
| Relatórios (dados pessoais) | `GET /api/relatorios/*` | ✗ | ✗ | ✗ | ✓ | RBAC + negar por padrão |
| Renovar empréstimo | `POST /api/emprestimos/{id}/renovar` | só o **próprio** | só o **próprio** | qualquer | ✗ | ABAC |
| Emprestar livro comum | `POST /api/emprestimos` | ✓ | ✓ | ✓ | ✗ | PBAC |
| Emprestar obra rara | `POST /api/emprestimos` | ✗ | ✓ | ✓ | ✗ | PBAC |

## As 4 falhas

| # | Falha | Onde está | Como demonstrar |
|---|---|---|---|
| 1 | **RBAC** — atributos não batem com a matriz | `Controllers/LivrosController.cs` | Ana (Leitor) cadastra livro → **201**; Beto (Bibliotecario) remove livro → **204** |
| 2 | **Negar por padrão** ausente | `Program.cs` + `Controllers/RelatoriosController.cs` | `GET /api/relatorios/leitores` **sem token** → 200 com CPF e telefone de todo mundo |
| 3 | **ABAC** que só olha o papel | `Autorizacao/DonoDoEmprestimo.cs` | Ana renova o empréstimo **102**, que é do Bruno → **200** |
| 4 | **PBAC** — política mal escrita | `politicas.json` | Ana (Leitor) empresta o livro **4** (obra rara) → **201** |

## Roteiro da prática (75 min, em duplas)

1. **Recon (10 min).** Suba a API, importe `BibliotecaApi.postman_collection.json` e rode a
   coleção inteira (*Run collection*). Os testes estão escritos com o resultado **correto**:
   os que falharem são as falhas. Anote, para cada uma, **qual dano** ela causa.
2. **Correção (50 min).** Corrija **uma falha por vez**, na ordem 1 → 4, e rode a coleção de novo
   após cada uma:
   - **Falha 1:** traduza a matriz para o código. Melhor ainda: crie **policies nomeadas** em
     `Program.cs` (`GerenciarAcervo`, `RemoverLivro`) e use `[Authorize(Policy = "...")]` — a regra
     fica num lugar só.
   - **Falha 2:** proteger só o relatório resolve o sintoma. Resolva a causa: faça a API **negar
     por padrão** (`FallbackPolicy`), de modo que o *próximo* endpoint esquecido também nasça
     fechado. Repare que os endpoints públicos já têm `[AllowAnonymous]` — por quê?
   - **Falha 3:** o handler precisa comparar um atributo do **sujeito** (claim `NameIdentifier` do
     token) com um atributo do **recurso** (`emprestimo.UsuarioId`). O Bibliotecário continua
     renovando qualquer um.
   - **Falha 4:** **não mexa em código C#.** O motor está correto; a política é que está errada.
     Leia em `MotorDePoliticas.cs` como ele combina as regras e corrija o `politicas.json`
     **com a API rodando** — a mudança vale na hora, sem recompilar nem reiniciar.
3. **Verificação cruzada (15 min).** Troque de máquina com outra dupla e tente furar as
   correções dela (dica: experimente o Bruno, a Carla e a Dora também).

**Entregável:** a coleção do Postman inteira verde (*Run collection*), com print do antes e do depois.

## RBAC × ABAC × PBAC — o que cada falha ensina

- **RBAC** (Falhas 1 e 2): a permissão vem do **papel**. Simples e auditável — desde que a matriz
  esteja no código **inteira** e que o esquecimento vire 403, não vazamento.
- **ABAC** (Falha 3): a decisão combina **atributos** do sujeito, do recurso, da ação e do contexto.
  "Professor pode lançar nota" é RBAC; "Professor lança nota **na sua turma**" é ABAC.
- **PBAC** (Falha 4): as regras viram um **documento de política** separado do código, avaliado por
  um motor (PDP) e aplicado pelo endpoint (PEP). Quem responde pela regra de negócio pode
  revisá-la e mudá-la sem um deploy. No mundo real: OPA/Rego, AWS Cedar, XACML.

O gabarito completo está em `GABARITO.txt` — consulte só depois de tentar.
