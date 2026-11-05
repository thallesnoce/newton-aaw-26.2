namespace LojaRevisao.Api.Modulos.Pedidos;

/// <summary>
/// Regras de negócio (elas são a verdade):
///   R1. Frete fixo de R$ 25,00.
///   R2. Frete GRÁTIS A PARTIR DE R$ 300,00 (R$ 300,00 inclusive).
///   R3. Total = subtotal + frete.
/// </summary>
public static class CalculadoraDePedido
{
    public const decimal FretePadrao = 25m;

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 11 · AULA 11 — Testes e qualidade (não corrija ainda!)
    //  Parte A — o código. A parte B está em LojaRevisao.Tests.
    //  Sintoma: quem compra exatamente R$ 300,00 ainda paga frete.
    //  Curiosidade: "dotnet test" passa inteiro, mesmo com o erro.
    //  Sua tarefa: primeiro conserte os testes (parte B) e veja um deles
    //  FICAR VERMELHO. Só então corrija esta regra, seguindo a R2 acima.
    // ══════════════════════════════════════════════════════════════════════
    public static decimal CalcularFrete(decimal subtotal) =>
        subtotal > 300m ? 0m : FretePadrao;

    public static decimal CalcularTotal(decimal subtotal) =>
        subtotal + CalcularFrete(subtotal);
}
