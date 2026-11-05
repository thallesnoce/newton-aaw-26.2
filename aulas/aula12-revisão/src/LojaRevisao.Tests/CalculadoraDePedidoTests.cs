using LojaRevisao.Api.Modulos.Pedidos;
using Xunit;

namespace LojaRevisao.Tests;

// ══════════════════════════════════════════════════════════════════════════
//  FALHA 11 · AULA 11 — Testes e qualidade (não corrija ainda!)
//  Parte B — os testes. A parte A está em Modulos/Pedidos/CalculadoraDePedido.cs.
//  Esta suíte está toda VERDE, e mesmo assim o código tem um bug. Dois motivos:
//    1. O esperado de uma borda foi copiado do que o código faz, não das regras.
//    2. Um dos testes executa o código, mas não verifica nada.
//  Sua tarefa: faça os testes obedecerem às regras R1 a R3. Rode "dotnet test",
//  veja o vermelho e só depois corrija o código.
// ══════════════════════════════════════════════════════════════════════════
public class CalculadoraDePedidoTests
{
    [Theory]
    [InlineData(100.00, 25.00)]
    [InlineData(299.99, 25.00)]
    [InlineData(300.00, 25.00)]
    [InlineData(300.01, 0.00)]
    public void CalcularFrete_ConformeSubtotal_RetornaFreteDaRegra(double subtotal, double freteEsperado)
    {
        var frete = CalculadoraDePedido.CalcularFrete((decimal)subtotal);

        Assert.Equal((decimal)freteEsperado, frete);
    }

    [Fact]
    public void CalcularTotal_CompraPequena_SomaOFrete()
    {
        var total = CalculadoraDePedido.CalcularTotal(100m);
    }
}
