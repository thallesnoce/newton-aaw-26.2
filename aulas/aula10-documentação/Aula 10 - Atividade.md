# HANDOUT — AULA 10

## Decifrando o Contrato

*Lendo uma especificação OpenAPI — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês receberam o contrato OpenAPI de uma API que NUNCA viram — e nada mais: sem código, sem wiki, sem ninguém para perguntar. Provem que o contrato basta:

- Parte 1 — Interroguem a especificação YAML do Card 1 e respondam às perguntas do Card 2
- Parte 2 — Comparem com a “documentação em prosa” do Card 3 e registrem o que só a spec responde

*⏱️ Tempo: 30 minutos  |  👥 Formato: em duplas  |  Só vale usar o que está impresso neste handout.*

> **Nomes:** ____________________   **Turma:** ____________________   **Data:** ___ / ___ / ______

## CARD 01 — O contrato — especificação OpenAPI (YAML)

Este é TODO o material que o time parceiro enviou. Leiam com calma — cada linha responde a uma pergunta do Card 2.

```text
openapi: 3.1.0
info:
  title: API Biblioteca
  version: 1.0.0
servers:
  - url: https://api.biblioteca.newton.br
paths:
  /api/v1/livros:
    get:
      summary: Lista os livros (paginado)
      parameters:
        - name: page
          in: query
          schema: { type: integer, default: 1 }
      responses:
        "200": { description: Lista de livros }
    post:
      summary: Cadastra um livro
      security:
        - bearerAuth: []
      requestBody:
        required: true
        content:
          application/json:
            schema: { $ref: "#/components/schemas/NovoLivro" }
      responses:
        "201": { description: Livro criado }
        "400": { description: Campos inválidos (ProblemDetails) }
        "401": { description: Token ausente ou inválido }
components:
  securitySchemes:
    bearerAuth:
      type: http
      scheme: bearer
      bearerFormat: JWT
  schemas:
    NovoLivro:
      type: object
      required: [titulo, autor]
      properties:
        titulo: { type: string, maxLength: 120 }
        autor:  { type: string }
        ano:    { type: integer, minimum: 1450 }
```

## CARD 02 — Interrogatório do contrato

Respondam usando SOMENTE o Card 1:

1. Que endpoints/operações existem e o que cada um faz?

2. Quais campos são OBRIGATÓRIOS para cadastrar um livro? Que restrições os campos têm?

3. Em que situações a API devolve 400? E 401? Qual operação exige token — e de que tipo?

4. Montem a requisição POST completa no papel (método, URL, headers, body JSON válido):

## CARD 03 — A mesma API, documentada “em prosa”

Versão que o parceiro tinha antes de adotar OpenAPI:

*“Para cadastrar um livro, envie os dados do livro para o endpoint de livros. É preciso estar autenticado. Os campos incluem título, autor e ano. Em caso de erro, a API retorna uma mensagem apropriada. Também é possível listar os livros cadastrados.”*

1. Listem 3 informações que vocês usaram no Card 2 e que esta prosa NÃO responde com precisão:

## DESAFIO

1. Mesmo com a spec em mãos, que pergunta a dupla AINDA precisaria fazer ao time da API? (Dica: o contrato diz o que a API promete — não diz tudo sobre como ela se comporta.)
