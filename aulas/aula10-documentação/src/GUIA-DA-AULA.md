# Aula: Documentação de APIs REST com OpenAPI 3.0

## Pré-requisitos

- .NET 6 SDK instalado (verificar com `dotnet --list-sdks`)
- Editor de código (VS Code, Visual Studio, Rider)
- Navegador web

## Como executar o projeto

```bash
cd BibliotecaApi
dotnet run
```

A API estará disponível em `http://localhost:<porta>` (a porta aparece no console).
Teste com: `curl http://localhost:5088/api/autores` (ajuste a porta).

---

## Parte 1 — Conhecendo a API (antes de documentar)

Antes de documentar, precisamos entender o que temos. Explore o projeto e responda:

### Exercício 1.1 — Mapeie os endpoints

Abra os 3 controllers em `Controllers/` e preencha esta tabela:

| Método HTTP | Rota | O que faz | Parâmetros |
|-------------|------|-----------|------------|
| ? | ? | ? | ? |

**Dica:** olhe os atributos `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`, `[HttpPatch]` e o `[Route]` da classe.

### Exercício 1.2 — Mapeie os modelos de dados

Abra os arquivos em `Models/` e identifique:

- Quais são as entidades?
- Quais campos são obrigatórios (`[Required]`)?
- Quais validações existem (`[StringLength]`, `[Range]`)?
- Qual a relação entre as entidades? (Autor → Livro → Empréstimo)

### Exercício 1.3 — Mapeie os códigos de resposta

Execute a API e teste com `curl` ou um cliente HTTP. Descubra:

- O que acontece ao buscar um autor que não existe? (qual código HTTP?)
- O que acontece ao tentar emprestar um livro já emprestado?
- O que acontece ao enviar um POST sem os campos obrigatórios?

---

## Parte 2 — Implementando o Swagger/OpenAPI

Agora que você conhece a API, vamos documentá-la de forma interativa.

### Passo 1 — Instalar o pacote Swashbuckle

No terminal, dentro da pasta `BibliotecaApi/`:

```bash
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
```

*(Fixe a versão: as versões mais novas do Swashbuckle não suportam .NET 6. A 6.5.0 é a
mesma da Aula 09 — se você fez aquela prática, o pacote já está no cache do NuGet.)*

**O que isso faz:** instala 3 componentes:
- `Swashbuckle.AspNetCore.Swagger` — gera o documento OpenAPI (JSON)
- `Swashbuckle.AspNetCore.SwaggerGen` — lê seus controllers e gera a especificação
- `Swashbuckle.AspNetCore.SwaggerUI` — interface visual interativa

Verifique que o pacote apareceu no `BibliotecaApi.csproj`.

---

### Passo 2 — Registrar os serviços do Swagger

Abra o arquivo `Program.cs` e encontre o comentário `TODO: Configurar Swagger/OpenAPI aqui`.

Substitua-o pelo seguinte código:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

**O que cada linha faz:**
- `AddEndpointsApiExplorer()` — permite que o Swagger descubra os endpoints da API
- `AddSwaggerGen()` — registra o gerador da especificação OpenAPI

---

### Passo 3 — Adicionar os middlewares do Swagger

No mesmo `Program.cs`, encontre o comentário `TODO: Adicionar middlewares do Swagger aqui`.

Substitua-o por:

```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

**O que cada linha faz:**
- `UseSwagger()` — expõe o JSON da especificação em `/swagger/v1/swagger.json`
- `UseSwaggerUI()` — expõe a interface visual em `/swagger/index.html`

---

### Passo 4 — Testar a documentação básica

```bash
dotnet run
```

Acesse no navegador: `http://localhost:<porta>/swagger`

**Observe e responda:**
- Os endpoints aparecem? Estão agrupados?
- Clique em um endpoint e depois em "Try it out" — funciona?
- Os modelos de dados aparecem na seção "Schemas" no final?
- O que está faltando na documentação? (descrições, exemplos, etc.)

---

### Passo 5 — Adicionar metadados da API

Volte ao `Program.cs` e melhore a configuração do `AddSwaggerGen`.

Substitua:
```csharp
builder.Services.AddSwaggerGen();
```

Por:
```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API da Biblioteca",
        Description = "API REST para gerenciamento do acervo e empréstimos de uma biblioteca.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipe de Desenvolvimento",
            Email = "dev@biblioteca.exemplo.com"
        }
    });
});
```

Rode `dotnet run` novamente e veja a diferença no topo da página do Swagger.

---

### Passo 6 — Habilitar XML Documentation Comments

Os comentários XML do C# (`/// <summary>`) podem alimentar o Swagger automaticamente.

**6a)** Edite o arquivo `BibliotecaApi.csproj` e adicione duas linhas dentro de `<PropertyGroup>`:

```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<NoWarn>$(NoWarn);1591</NoWarn>
```

O `<PropertyGroup>` deve ficar assim:
```xml
<PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

**O que isso faz:**
- `GenerateDocumentationFile` — gera um arquivo `.xml` com todos os comentários `///` do código
- `NoWarn 1591` — silencia avisos de "falta de comentário XML" (senão cada classe sem `///` gera warning)

**6b)** No `Program.cs`, dentro do `AddSwaggerGen(options => { ... })`, adicione ao final do bloco:

```csharp
    // Inclui os comentários XML na documentação Swagger
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
```

Rode novamente — por enquanto nada muda, porque ainda não escrevemos os comentários XML.

---

### Passo 7 — Documentar os Models com XML Comments

Abra `Models/Autor.cs` e adicione comentários XML acima da classe e de cada propriedade:

```csharp
/// <summary>
/// Representa um autor de livros no sistema da biblioteca.
/// </summary>
public class Autor
{
    /// <summary>
    /// Identificador único do autor.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome completo do autor.
    /// </summary>
    /// <example>Machado de Assis</example>
    [Required(ErrorMessage = "O nome do autor é obrigatório.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    // ... continue para as demais propriedades
}
```

**Tags importantes:**
- `<summary>` — descrição que aparece no Swagger
- `<example>` — valor de exemplo que preenche o "Try it out"

**Exercício:** Faça o mesmo para `Livro.cs`, `Emprestimo.cs` e `RespostaPaginada.cs`.

Rode `dotnet run` e observe como os schemas no Swagger agora mostram descrições e exemplos.

---

### Passo 8 — Documentar os Controllers

Abra `Controllers/AutoresController.cs` e adicione documentação.

**8a)** Adicione atributos na classe:

```csharp
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Autores")]
public class AutoresController : ControllerBase
```

- `[Produces("application/json")]` — indica o content-type das respostas
- `[Tags("Autores")]` — agrupa os endpoints no Swagger sob esse nome

**8b)** Documente cada action com XML comments e `[ProducesResponseType]`:

```csharp
/// <summary>
/// Lista todos os autores cadastrados.
/// </summary>
/// <returns>Lista de autores com seus livros associados.</returns>
/// <response code="200">Retorna a lista de autores.</response>
[HttpGet]
[ProducesResponseType(typeof(List<Autor>), StatusCodes.Status200OK)]
public IActionResult ObterTodos()
```

```csharp
/// <summary>
/// Obtém um autor específico pelo seu ID.
/// </summary>
/// <param name="id">ID do autor.</param>
/// <returns>Dados do autor encontrado.</returns>
/// <response code="200">Autor encontrado.</response>
/// <response code="404">Autor não encontrado.</response>
[HttpGet("{id:int}")]
[ProducesResponseType(typeof(Autor), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public IActionResult ObterPorId(int id)
```

```csharp
/// <summary>
/// Cadastra um novo autor.
/// </summary>
/// <param name="autor">Dados do autor a ser cadastrado.</param>
/// <returns>Autor recém-criado.</returns>
/// <response code="201">Autor criado com sucesso.</response>
/// <response code="400">Dados inválidos.</response>
[HttpPost]
[ProducesResponseType(typeof(Autor), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public IActionResult Criar([FromBody] Autor autor)
```

**Tags importantes:**
- `<param>` — descreve cada parâmetro
- `<response>` — descreve cada código de resposta possível
- `[ProducesResponseType]` — declara os tipos de resposta para o Swagger

**Exercício:** Faça o mesmo para `LivrosController.cs` e `EmprestimosController.cs`.

Dica para o `EmprestimosController`, use também a tag `<remarks>` para exemplos mais ricos:

```csharp
/// <summary>
/// Realiza um novo empréstimo de livro.
/// </summary>
/// <remarks>
/// O livro deve estar disponível (não emprestado).
/// O prazo de devolução padrão é de 14 dias.
///
/// Exemplo de requisição:
///
///     POST /api/emprestimos
///     {
///         "livroId": 1,
///         "nomeUsuario": "João Silva"
///     }
///
/// </remarks>
```

---

### Passo 9 — Documentar a segurança (security scheme + botão Authorize)

Repare: os endpoints de **Empréstimos** têm `[Authorize]` — o "Try it out" neles
retorna **401**. A API usa JWT (tema da Aula 08; aqui já vem pré-pronto no
`AuthController` e no `Program.cs`). Falta o contrato DIZER isso ao consumidor.

**9a)** No `Program.cs`, dentro do `AddSwaggerGen(options => { ... })`, adicione:

```csharp
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Informe o token JWT obtido em POST /api/auth/login."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
```

*(Código testado com .NET 6 + Swashbuckle.AspNetCore 6.5.0. O `Reference` aponta para o
scheme "Bearer" declarado acima — é o `$ref` que você viu nos slides.)*

**9b)** Rode a API e teste o fluxo completo no Swagger UI:

1. `POST /api/auth/login` com `{ "usuario": "bibliotecario", "senha": "newton123" }` → copie o token
2. Clique no botão **Authorize** (cadeado no topo) e cole o token
3. Repita o "Try it out" em `GET /api/emprestimos` — agora **200**

**Pergunta:** onde essa exigência de token aparece no `/swagger/v1/swagger.json`?
Procure por `securitySchemes` e `security`.

---

### Passo 10 — Swagger UI na raiz do site (opcional)

Para que o Swagger abra direto ao acessar `http://localhost:<porta>/`:

```csharp
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API da Biblioteca v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle = "Documentação - API da Biblioteca";
});
```

---

### Passo 11 — Exportar o contrato e importar no Postman

O contrato não serve só para a UI — ele é um ARQUIVO que circula entre times:

1. Com a API rodando, baixe a especificação: `http://localhost:<porta>/swagger/v1/swagger.json`
   (salve como `openapi.json`)
2. No Postman: **Import** → selecione o `openapi.json` → o Postman gera a
   **coleção inteira** automaticamente, com todos os endpoints e exemplos
3. Compare com a coleção que você montou À MÃO nas aulas anteriores

**Entregável da prática:** Swagger UI completo (com Authorize funcionando) +
coleção do Postman importada do `openapi.json`.

---

## Parte 3 — Verificação final

Depois de implementar tudo, rode `dotnet run` e confira no Swagger UI:

| Item | Onde verificar |
|------|---------------|
| Título e descrição da API | Topo da página |
| Endpoints agrupados por tag | Seções "Autores", "Livros", "Empréstimos" |
| Descrição de cada endpoint | Expandir qualquer endpoint |
| Parâmetros documentados | Seção "Parameters" de cada endpoint |
| Códigos de resposta listados | Seção "Responses" de cada endpoint |
| Exemplos nos models | Seção "Schemas" no final da página |
| "Try it out" funcional | Botão dentro de qualquer endpoint |
| Cadeado nos endpoints protegidos | Seção "Empréstimos" (após o Passo 9) |
| Botão Authorize funcionando | Login → token → Authorize → 200 |
| JSON da especificação | Acessar `/swagger/v1/swagger.json` |

---

## Parte 4 — Diagramas de arquitetura

Na pasta `Diagramas/` há 4 diagramas em formato Mermaid.
Copie o conteúdo de cada arquivo e cole em [mermaid.live](https://mermaid.live) para visualizar:

| Arquivo | O que mostra |
|---------|-------------|
| `01-arquitetura-geral.md` | Componentes da API e como se conectam |
| `02-modelo-de-dados.md` | Entidade-Relacionamento (Autor → Livro → Empréstimo) |
| `03-fluxo-emprestimo.md` | Diagrama de sequência do empréstimo e devolução |
| `04-endpoints-rest.md` | Mapa visual de todos os endpoints e códigos HTTP |

---

## Referência rápida — Tags XML para Swagger

| Tag | Onde usar | O que gera no Swagger |
|-----|-----------|----------------------|
| `<summary>` | Classes e métodos | Descrição principal |
| `<param name="x">` | Métodos | Descrição do parâmetro |
| `<returns>` | Métodos | Descrição da resposta |
| `<response code="200">` | Métodos | Texto do código HTTP |
| `<remarks>` | Métodos | Texto expandido com exemplos |
| `<example>` | Propriedades | Valor de exemplo no schema |

## Referência rápida — Atributos C# para Swagger

| Atributo | O que faz |
|----------|-----------|
| `[Tags("Nome")]` | Agrupa endpoints sob um nome no Swagger |
| `[Produces("application/json")]` | Declara content-type da resposta |
| `[ProducesResponseType(typeof(T), 200)]` | Declara tipo e código da resposta |
| `[ProducesResponseType(404)]` | Declara código de resposta sem body |
