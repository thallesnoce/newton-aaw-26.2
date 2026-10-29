using FluentAssertions;
using Moq;
using ResilientOrders.Api.Models;
using ResilientOrders.Api.Services;
using Xunit;

namespace ResilientOrders.UnitTests;

// =====================================================================
//  AULA 11 — CAÇADA AO BUG
//
//  As regras do PedidoService (as mesmas do handout):
//    R1. Item com quantidade <= 0 torna o pedido inválido.
//    R2. Desconto: 5% a partir de R$200 (inclusive)
//                 10% a partir de R$500 (inclusive)
//    R3. Frete: R$25 fixo; grátis a partir de R$300 (inclusive).
//    R4. Total = subtotal - desconto + frete.
//
//  Sua missão (em dupla):
//    1. Transforme a tabela de casos do handout em testes [Theory]/[InlineData].
//    2. Rode `dotnet test`. Se algum caso de BORDA falhar... você achou o bug!
//    3. Corrija o PedidoService e rode tudo de novo até ficar verde.
//
//  Padrão AAA em todos os testes: Arrange -> Act -> Assert.
// =====================================================================

public class PedidoServiceTests
{
    private static PedidoService CriarService()
    {
        var mockRepo = new Mock<IPedidoRepositorio>();
        mockRepo.Setup(r => r.Salvar(It.IsAny<Pedido>())).Returns<Pedido>(p => p);
        return new PedidoService(mockRepo.Object);
    }

    // ---------------------------------------------------------------
    //  EXEMPLO PRONTO — use como modelo para os demais.
    //  Caminho feliz: subtotal 100 -> sem desconto (R2), frete 25 (R3)
    //  Total esperado: 100 - 0 + 25 = 125 (R4)
    // ---------------------------------------------------------------
    [Fact(DisplayName = "CalcularTotal: subtotal 100 não tem desconto e paga frete")]
    public void CalcularTotal_Subtotal100_Retorna125()
    {
        // Arrange
        var service = CriarService();

        // Act
        var total = service.CalcularTotal(100m);

        // Assert
        total.Should().Be(125m);
    }

    // ---------------------------------------------------------------
    //  TODO 1 — Complete com a tabela de casos do handout.
    //  Cada InlineData é uma linha da tabela: (subtotal, totalEsperado).
    //  Não esqueça as BORDAS EXATAS: 199.99, 200, 299.99, 300, 499.99, 500!
    //  Remova o Skip quando implementar.
    // ---------------------------------------------------------------
    [Theory(Skip = "TODO: implementar os casos da tabela do handout")]
    [InlineData(100, 125)]   // exemplo — adicione as demais linhas
    public void CalcularTotal_TabelaDoHandout_RetornaTotalEsperado(decimal subtotal, decimal totalEsperado)
    {
        // Arrange
        // TODO: criar o service (use CriarService())

        // Act
        // TODO: calcular o total

        // Assert
        // TODO: total.Should().Be(totalEsperado);
    }

    // ---------------------------------------------------------------
    //  TODO 2 — R1: item com quantidade <= 0 deve lançar ArgumentException
    //  e o repositório NÃO deve ser chamado (Times.Never).
    // ---------------------------------------------------------------
    [Fact(Skip = "TODO: implementar validação de quantidade inválida")]
    public void CriarPedido_ComQuantidadeZero_LancaArgumentException()
    {
        // Arrange
        // TODO: mock do IPedidoRepositorio + request com item Quantidade = 0

        // Act
        // TODO: Action act = () => service.CriarPedido(request);

        // Assert
        // TODO: act.Should().Throw<ArgumentException>();
        // TODO: mockRepo.Verify(r => r.Salvar(It.IsAny<Pedido>()), Times.Never);
    }

    // ---------------------------------------------------------------
    //  TODO 3 — pedido sem itens deve lançar InvalidOperationException.
    // ---------------------------------------------------------------
    [Fact(Skip = "TODO: implementar validação de pedido vazio")]
    public void CriarPedido_SemItens_LancaInvalidOperationException()
    {
        // Arrange / Act / Assert — siga o padrão dos anteriores.
    }

    // ---------------------------------------------------------------
    //  DESAFIO BÔNUS — CriarPedido válido chama Salvar exatamente 1 vez
    //  e o Pedido devolvido tem Subtotal, Desconto, Frete e Total coerentes.
    // ---------------------------------------------------------------
}
