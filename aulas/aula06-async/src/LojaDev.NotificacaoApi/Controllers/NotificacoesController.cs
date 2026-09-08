using Microsoft.AspNetCore.Mvc;

namespace LojaDev.NotificacaoApi.Controllers;

/// <summary>
/// Serviço de Notificação — PRONTO (não precisa alterar).
/// 
/// Simula o envio de e-mail/SMS com latência ALTA (~3 segundos)
/// e falhas aleatórias (~20% das vezes), representando um provedor
/// de e-mail externo instável.
///
/// Essa lentidão e instabilidade é PROPOSITAL — é o que justifica
/// usar comunicação ASSÍNCRONA para este serviço.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificacoesController : ControllerBase
{
    private static readonly Random _random = new();

    /// <summary>
    /// POST /api/notificacoes
    /// Recebe dados e "envia" uma notificação (simulação).
    /// Latência simulada: ~3 segundos.
    /// Taxa de falha: ~20%.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> EnviarNotificacao([FromBody] NotificacaoRequest request)
    {
        Console.WriteLine($"📧 Enviando notificação para {request.Destinatario}...");
        var inicio = DateTime.Now;

        // Simula latência alta do provedor de e-mail (~3 segundos)
        await Task.Delay(3000);

        // Simula falha em ~20% das vezes (provedor instável)
        bool falhou = _random.Next(1, 6) == 1; // 1 em 5 = 20%

        var tempo = (DateTime.Now - inicio).TotalMilliseconds;

        if (falhou)
        {
            Console.WriteLine($"📧 ❌ FALHA ao notificar {request.Destinatario} ({tempo:F0}ms)");
            return StatusCode(503, new
            {
                sucesso = false,
                mensagem = "Provedor de e-mail indisponível. Tente novamente.",
                tempoProcessamentoMs = tempo
            });
        }

        Console.WriteLine($"📧 ✅ Notificação enviada para {request.Destinatario} ({tempo:F0}ms)");
        return Ok(new
        {
            sucesso = true,
            mensagem = $"E-mail enviado para {request.Destinatario}: {request.Assunto}",
            tempoProcessamentoMs = tempo
        });
    }
}

public class NotificacaoRequest
{
    public string Destinatario { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
    public string Corpo { get; set; } = string.Empty;
}
