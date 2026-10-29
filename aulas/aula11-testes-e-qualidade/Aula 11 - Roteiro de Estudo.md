# ROTEIRO DE ESTUDO — AULA 11

## Testes e Qualidade dos Serviços

*Sala de aula invertida — estudo autônomo em sala — Arquitetura de Aplicações Web*

## 🎯 COMO FUNCIONA A AULA 11

A Aula 11 é **invertida**: hoje o professor não começa explicando. A turma é dividida em **3 grupos**; cada grupo estuda UMA parte do texto-base e a apresenta, rapidamente, para os outros dois. Quem ensina é a turma.

- **Bloco 1:** estudo da parte do seu grupo (20 min), preparo da apresentação (8 min) e apresentações dos 3 grupos (4 min cada)
- **Bloco 2:** teste de prontidão sobre TODO o texto-base — primeiro individual, depois em equipes misturando as 3 partes, sem consulta — e casos práticos em equipe
- **Blocos 3 e 4:** atividade em papel (caçada ao bug) e prática no projeto `ResilientOrders` com xUnit

*⚠️ O teste de prontidão cobre as 3 partes. Você estuda a sua; as outras duas você aprende com a apresentação dos colegas — e os colegas aprendem a sua com você. Prestem atenção uns nos outros.*

## 🧩 AS 3 PARTES

| Grupo | Parte | Seções do texto-base | Você deve saber responder |
| --- | --- | --- | --- |
| **1** | **Por que e onde testar** | 1 (Por que testar), 2 (A pirâmide de testes) e 7 (O que não testar — e o mito da cobertura) | Por que a pirâmide tem MUITOS unitários e POUCOS E2E? Por que 95% de cobertura não significa “bem testado”? |
| **2** | **Como escrever um teste** | 3 (Anatomia de um teste com xUnit) e 4 ([Theory] e a análise de valor-limite) | O que vai em cada parte do AAA? Quando usar `[Fact]` e quando usar `[Theory]`? Dada a regra “a partir de R$ 300”, quais valores testar? |
| **3** | **Isolar e integrar** | 5 (Isolando dependências: interfaces e mocks) e 6 (Teste de integração com WebApplicationFactory) | Para que serve um mock? O que `Verify(..., Times.Never)` verifica? O que o `WebApplicationFactory` faz e por que é integração, não unitário? |

## 🗺️ ROTEIRO DO BLOCO 1

| # | O quê | Tempo |
| --- | --- | --- |
| 1 | **Antes de ler, prepare o ambiente:** atualize o repositório da disciplina e rode `dotnet test` em `aulas/aula 11 - testes e qualidade/src`. Deixe rodando enquanto lê — o primeiro build baixa os pacotes NuGet | 2 min |
| 2 | Ler, individualmente, as seções da parte do SEU grupo. Anote dúvidas | 18 min |
| 3 | **No grupo:** montar a apresentação no formato abaixo | 8 min |
| 4 | **Apresentações:** grupo 1, depois 2, depois 3 — 4 min cada (3 min de fala + 1 min de pergunta da turma) | 15 min |

*💡 Resultado esperado do passo 1: 2 testes aprovados e 4 ignorados (Skipped), sem nenhuma falha. Se der erro de rede ao baixar pacotes, avise o professor já — não espere a prática.*

## 🎤 FORMATO DA APRESENTAÇÃO (4 min por grupo)

- **3 ideias-chave** da parte — o que a turma PRECISA levar
- **1 exemplo** — um trecho de código do texto-base, escrito no quadro, explicado linha a linha
- **1 pergunta para a turma** — que a resposta só sabe quem prestou atenção
- **Quem apresenta é sorteado na hora:** todo o grupo tem de estar pronto para falar

## ✅ ANTES DO TESTE, CONFIRA SE CONSEGUE RESPONDER

1. Por que a pirâmide de testes tem MUITOS unitários e POUCOS E2E?
2. O que vai em cada parte de um teste Arrange-Act-Assert?
3. Quando usar `[Fact]` e quando usar `[Theory]` + `[InlineData]`?
4. Dada uma regra como “a partir de R$ 300”, quais valores você testaria? Por quê?
5. Para que serve um mock? O que `Verify(..., Times.Never)` verifica?
6. O que o `WebApplicationFactory` faz e por que ele é teste de INTEGRAÇÃO, não unitário?
7. Por que 95% de cobertura não significa “bem testado”?

## 📚 PARA APROFUNDAR (depois da aula, ou se terminar antes)

- VOCKE, Ham. The Practical Test Pyramid — seções “The Test Pyramid”, “Unit tests” e “Integration Tests”: https://martinfowler.com/articles/practical-test-pyramid.html
- MICROSOFT. Práticas recomendadas de teste de unidade (.NET) — nomenclatura, AAA e “evitar lógica nos testes”: https://learn.microsoft.com/pt-br/dotnet/core/testing/unit-testing-best-practices
- MICROSOFT. Testes de integração no ASP.NET Core — seção sobre `WebApplicationFactory`: https://learn.microsoft.com/pt-br/aspnet/core/test/integration-tests
- xUnit.net (consulta): https://xunit.net/

## TEXTO-BASE

### 1. Por que testar automaticamente  ·  *Grupo 1*

Um teste automatizado é um código que executa outro código e verifica se o resultado é o esperado. Ele serve para três coisas:

- **Rede de segurança:** você altera o código e, em segundos, `dotnet test` diz se algo que funcionava quebrou — antes do cliente perceber.
- **Documentação executável:** um teste chamado `CalcularFrete_Subtotal300_FreteGratis` explica a regra melhor que um comentário, e nunca fica desatualizado (se ficar, ele falha).
- **Pressão por bom design:** código difícil de testar costuma ser código acoplado. Se para testar uma regra você precisa subir um banco de dados, a regra está no lugar errado.

### 2. A pirâmide de testes  ·  *Grupo 1*

| Camada | O que testa | Velocidade | Quantidade |
| --- | --- | --- | --- |
| **E2E / UI** | O sistema inteiro, pela interface, como o usuário | Segundos a minutos; frágil | Poucos |
| **Integração** | Várias peças juntas: rota HTTP + controller + serviço + serialização | Centenas de milissegundos | Alguns |
| **Unitário** | Uma unidade isolada (um método, uma classe), sem banco nem rede | Milissegundos | Muitos |

Quanto mais alto na pirâmide, mais caro escrever, mais lento rodar e mais difícil descobrir ONDE está o erro quando o teste falha. Por isso a base é larga. Uma suíte com muitos E2E e poucos unitários é a **pirâmide invertida** (ou “casquinha de sorvete”): lenta, instável, e ninguém a roda antes do commit.

**Regra prática:** um teste que abre conexão com banco de dados, chama outra API ou lê arquivo do disco **não é unitário** — é de integração, mesmo que esteja no projeto `UnitTests`.

### 3. Anatomia de um teste com xUnit  ·  *Grupo 2*

O padrão **Arrange-Act-Assert (AAA)** organiza todo teste em três partes:

```csharp
[Fact] // um caso único, sem parâmetros
public void Aprovar_Nota7_RetornaAprovado()
{
    // Arrange — prepara o cenário (objetos, dados de entrada)
    var avaliador = new Avaliador();

    // Act — executa UMA ação, a que está sendo testada
    var situacao = avaliador.Aprovar(7.0m);

    // Assert — verifica o resultado
    Assert.Equal("Aprovado", situacao);
}
```

- O nome segue o padrão **Metodo_Cenario_ResultadoEsperado**: quem lê o nome do teste que falhou já sabe o que quebrou.
- Um teste **sem Assert** sempre passa — inclusive com o código errado. Isso é pior que não ter teste: dá falsa confiança.
- Para verificar exceções: `Assert.Throws<ArgumentException>(() => avaliador.Aprovar(-1m));`

Na prática da aula usaremos também a biblioteca **FluentAssertions**, que escreve o Assert em forma de frase. As duas linhas abaixo verificam a mesma coisa:

```csharp
Assert.Equal(125m, total);
total.Should().Be(125m);
```

### 4. [Theory] e a análise de valor-limite  ·  *Grupo 2*

Quando a mesma verificação precisa rodar para várias entradas, use `[Theory]`. Cada `[InlineData]` é um caso — uma linha de uma tabela de casos de teste:

```csharp
[Theory]
[InlineData(5.99, "Reprovado")]  // logo abaixo da borda
[InlineData(6.00, "Aprovado")]   // exatamente na borda
[InlineData(10.0, "Aprovado")]   // caminho feliz
public void Aprovar_DiversasNotas_RetornaSituacao(decimal nota, string esperado)
{
    var avaliador = new Avaliador();
    var situacao = avaliador.Aprovar(nota);
    Assert.Equal(esperado, situacao);
}
```

Regra: “aprovado com nota **a partir de** 6,0”. Se o programador escreveu `nota > 6` em vez de `nota >= 6`, os casos 10,0 e 5,99 passam normalmente — só o caso **exatamente 6,00** revela o erro. Esta é a **análise de valor-limite**: bugs se escondem nas fronteiras. Para cada limite de uma regra, teste o valor exato da borda e o vizinho imediatamente do outro lado.

**Atenção:** o resultado esperado vem da **regra de negócio**, nunca do código. Se você calcular o esperado copiando a fórmula do código (`var esperado = nota > 6 ? ...`), o teste copia o bug junto e passa.

### 5. Isolando dependências: interfaces e mocks  ·  *Grupo 3*

Imagine um `PedidoService` que, ao criar um pedido, chama `IPedidoRepositorio.Salvar(...)`. Para testar a regra do serviço de forma unitária, não queremos um banco real. Como o serviço depende de uma **interface**, no teste podemos entregar a ele um **mock** — um objeto falso, controlado pelo teste. Com a biblioteca **Moq**:

```csharp
// Arrange — um repositório falso
var mockRepo = new Mock<IPedidoRepositorio>();
mockRepo.Setup(r => r.Salvar(It.IsAny<Pedido>()))
        .Returns<Pedido>(p => p);           // "quando chamarem Salvar, devolva o próprio pedido"
var service = new PedidoService(mockRepo.Object);

// ... Act e Assert ...

// Verifica a INTERAÇÃO: Salvar foi chamado exatamente uma vez?
mockRepo.Verify(r => r.Salvar(It.IsAny<Pedido>()), Times.Once);
```

- `Setup(...).Returns(...)` define o que o mock responde.
- `Verify(..., Times.Once)` / `Times.Never` verifica se o método foi chamado (e quantas vezes). Exemplo: um pedido inválido deve lançar exceção **e** o repositório **nunca** deve ser chamado — `Times.Never`.

### 6. Teste de integração com WebApplicationFactory  ·  *Grupo 3*

O `WebApplicationFactory<Program>` (pacote `Microsoft.AspNetCore.Mvc.Testing`) sobe a **API inteira em memória** — rotas, injeção de dependência, validação, serialização JSON — e devolve um `HttpClient` que faz requisições HTTP de verdade, sem abrir porta na máquina:

```csharp
var factory = new WebApplicationFactory<Program>();
var client = factory.CreateClient();

var resposta = await client.PostAsJsonAsync("/api/pedidos", payload);

Assert.Equal(HttpStatusCode.Created, resposta.StatusCode); // 201 — o contrato HTTP
```

- É o teste que pega erros que nenhum teste unitário do serviço pegaria: rota errada, controller devolvendo 200 em vez de 201, JSON com nome de campo trocado.
- Para funcionar com *top-level statements*, o `Program.cs` precisa terminar com `public partial class Program { }` — senão o projeto de teste não “enxerga” a classe `Program`.

### 7. O que não testar — e o mito da cobertura  ·  *Grupo 1*

- **Não teste o framework:** o ASP.NET já foi testado pela Microsoft. Não escreva teste para provar que `[HttpGet]` roteia.
- **Não teste o trivial:** getters/setters sem lógica e construtores que só atribuem campos.
- **Teste comportamento, não implementação:** o teste deve dizer O QUE o código faz (a regra), não COMO (quais linhas ele executa).
- **Cobertura é termômetro, não meta:** a cobertura mede quais linhas foram EXECUTADAS durante os testes, não se alguma coisa foi VERIFICADA. Cem testes sem Assert podem dar 100% de cobertura e não proteger nada.

## 📖 GLOSSÁRIO RÁPIDO

| Termo | Significado |
| --- | --- |
| **AAA** | Arrange-Act-Assert: preparar, executar, verificar |
| **[Fact]** | Teste xUnit sem parâmetros — um caso |
| **[Theory] + [InlineData]** | Teste xUnit parametrizado — um caso por InlineData |
| **Valor-limite (borda)** | A fronteira de uma regra (“a partir de”, “até”), onde moram os bugs |
| **Mock** | Objeto falso que substitui uma dependência e registra como foi usado |
| **Times.Never / Times.Once** | Verificação do Moq: o método foi chamado nenhuma / uma vez |
| **WebApplicationFactory** | Sobe a API em memória para testes de integração via HTTP |
| **Cobertura** | Percentual de linhas executadas pela suíte — não mede qualidade das asserções |
