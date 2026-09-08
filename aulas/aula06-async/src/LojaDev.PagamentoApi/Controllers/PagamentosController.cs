using LojaDev.Compartilhado.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace LojaDev.PagamentoApi.Controllers;

/// <summary>
/// Serviço de Pagamento — PRONTO (não precisa alterar).
/// 
/// Simula a aprovação de um pagamento com latência de ~1 segundo,
/// representando o tempo de consulta a uma operadora de cartão.
/// Pedidos acima de R$ 10.000 são rejeitados (simulação de limite).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PagamentosController : ControllerBase
{
    /// <summary>
    /// POST /api/pagamentos
    /// Recebe um pedido e retorna aprovação ou rejeição.
    /// Latência simulada: ~1 segundo.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ProcessarPagamento([FromBody] Pedido pedido)
    {
        Console.WriteLine($"💳 Processando pagamento do pedido {pedido.Id}...");
        var inicio = DateTime.Now;

        // Simula latência da operadora de cartão (~1 segundo)
        await Task.Delay(1000);

        // Regra simples: rejeita pedidos acima de R$ 10.000
        bool aprovado = pedido.Valor <= 10_000;
        var tempo = (DateTime.Now - inicio).TotalMilliseconds;

        Console.WriteLine(aprovado
            ? $"💳 ✅ Pagamento APROVADO — Pedido {pedido.Id} — R$ {pedido.Valor} ({tempo:F0}ms)"
            : $"💳 ❌ Pagamento REJEITADO — Pedido {pedido.Id} — R$ {pedido.Valor} ({tempo:F0}ms)");

        return Ok(new
        {
            pedidoId = pedido.Id,
            aprovado,
            mensagem = aprovado
                ? "Pagamento aprovado pela operadora"
                : "Pagamento rejeitado: valor acima do limite de R$ 10.000",
            tempoProcessamentoMs = tempo
        });
    }
}
