using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace ResilientOrders.IntegrationTests;

// =====================================================================
//  AULA 11 — ETAPA 4: Teste de integração com WebApplicationFactory
//
//  A fixture (ApiFactoryFixture) já sobe a API INTEIRA em memória:
//  requisição HTTP de verdade, roteamento, controller, service — tudo,
//  sem abrir porta nenhuma. Use _fixture.HttpClient.
// =====================================================================

[Collection("api")]
public class PedidosEndpointTests
{
    private readonly ApiFactoryFixture _fixture;

    public PedidosEndpointTests(ApiFactoryFixture fixture)
    {
        _fixture = fixture;
    }

    // ---------------------------------------------------------------
    //  EXEMPLO PRONTO — GET /api/pedidos responde 200.
    // ---------------------------------------------------------------
    [Fact(DisplayName = "GET /api/pedidos retorna 200 OK")]
    public async Task GetPedidos_Retorna200()
    {
        // Act
        var response = await _fixture.HttpClient.GetAsync("/api/pedidos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ---------------------------------------------------------------
    //  TODO — POST /api/pedidos: 201 com payload válido, 400 sem itens.
    //  Payload válido de referência:
    //    new { nomeCliente = "Maria", itens = new[] {
    //        new { produtoId = 1, nomeProduto = "Mouse", precoUnitario = 50m, quantidade = 2 }
    //    } }
    //  Dicas:
    //    - PostAsJsonAsync("/api/pedidos", payload)
    //    - 201: response.StatusCode.Should().Be(HttpStatusCode.Created)
    //           e response.Headers.Location.Should().NotBeNull()
    //    - 400: payload com itens = new object[0]
    //           e o corpo deve conter "vazio"
    //  Remova o Skip quando implementar.
    // ---------------------------------------------------------------
    [Fact(Skip = "TODO: implementar POST 201 (válido) e 400 (sem itens)")]
    public async Task PostPedidos_Valido201_Invalido400()
    {
        // Arrange / Act / Assert
    }
}
