# Aula 10 — Documentação de APIs (OpenAPI/Swagger) — Prática

Template da prática "Operação Contrato": uma API de Biblioteca **funcional, mas sem
documentação** — o trabalho da aula é documentá-la com OpenAPI/Swagger até o contrato
completo (incluindo segurança) e exportá-lo para o Postman.

## Conteúdo desta pasta

| Item | O que é |
|------|---------|
| `BibliotecaApi/` | A API .NET (Autores, Livros, Empréstimos) — roda com `dotnet run` |
| `GUIA-DA-AULA.md` | Roteiro passo a passo da prática (Passos 1–11) |
| `Diagramas/` | 4 diagramas Mermaid da arquitetura (usar em mermaid.live) |
| `GABARITO-Aula10.txt` | Gabarito completo (uso do professor) |

## Estado inicial (de propósito)

- API completa e funcional, **zero documentação** (TODOs marcados no `Program.cs`)
- Endpoints de **Empréstimos** protegidos por JWT **pré-pronto** (tema da Aula 08 — não é
  o foco; credenciais didáticas: `bibliotecario` / `newton123`) — eles existem para o
  Passo 9 (security scheme + botão Authorize) fazer sentido

## Roteiro (resumo — detalhes no GUIA-DA-AULA.md)

1. Conhecer a API (mapear endpoints, modelos e códigos de resposta)
2. Passos 1–4: Swashbuckle + Swagger UI básico
3. Passos 5–8: metadados, XML comments, `[ProducesResponseType]`
4. Passo 9: security scheme Bearer + fluxo login → Authorize → 200
5. Passos 10–11: UI na raiz (opcional) e export do `openapi.json` → import no Postman

## Entregável

Swagger UI completo (com Authorize funcionando nos endpoints protegidos) + coleção do
Postman gerada a partir do `openapi.json`.

## Requisitos

.NET 6 SDK (projeto em `net6.0`; Swashbuckle 6.5.0, instalado no Passo 1), VSCode, Postman,
navegador.
