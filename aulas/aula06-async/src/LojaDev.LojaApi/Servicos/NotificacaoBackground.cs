using System.Text;
using System.Text.Json;
using LojaDev.Compartilhado.Fila;
using LojaDev.Compartilhado.Modelos;

namespace LojaDev.LojaApi.Servicos;

/// <summary>
/// Serviço de background que consome eventos da fila e envia
/// notificações chamando a NotificacaoApi.
///
/// BackgroundService roda em uma thread separada da API, sem
/// bloquear as requisições HTTP. Ele fica "escutando" a fila
/// e processa cada evento assim que chega.
///
/// Em produção, este seria um worker/consumidor separado
/// conectado a um broker de mensagens (RabbitMQ, Azure Service Bus, etc.).
///
/// ══════════════════════════════════════════════════════════════
///  ATENÇÃO: Complete os TODOs 6 e 7 neste arquivo.
/// ══════════════════════════════════════════════════════════════
/// </summary>
public class NotificacaoBackground : BackgroundService
{
    private readonly FilaDePedidos _fila;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IConfiguration _config;

    public NotificacaoBackground(
        FilaDePedidos fila,
        IHttpClientFactory httpFactory,
        IConfiguration config)
    {
        _fila = fila;
        _httpFactory = httpFactory;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("🔄 NotificacaoBackground iniciado — aguardando eventos na fila...");

        // ╔═══════════════════════════════════════════════════════╗
        // ║  TODO 6: Consumir eventos da fila                    ║
        // ║                                                      ║
        // ║  Use _fila.ConsumirAsync(stoppingToken) em um        ║
        // ║  "await foreach" para ler cada evento da fila.       ║
        // ║                                                      ║
        // ║  Para cada evento recebido:                          ║
        // ║  1. Logue que recebeu o evento                       ║
        // ║  2. Chame o método ProcessarEvento (TODO 7)          ║
        // ╚═══════════════════════════════════════════════════════╝

        // await foreach (var evento in _fila.ConsumirAsync(stoppingToken))
        // {
        //     Console.WriteLine($"📥 [BACKGROUND] Evento recebido — Pedido {evento.PedidoId}");
        //     await ProcessarEvento(evento);
        // }

        // ⬇⬇⬇ REMOVA ESTE BLOCO APÓS COMPLETAR O TODO 6 ⬇⬇⬇
        Console.WriteLine("⚠️  TODO 6 não implementado — background não consome a fila");
        await Task.Delay(Timeout.Infinite, stoppingToken);
        // ⬆⬆⬆ REMOVA ESTE BLOCO APÓS COMPLETAR O TODO 6 ⬆⬆⬆
    }

    /// <summary>
    /// Processa um evento da fila: chama NotificacaoApi via HTTP.
    /// Se falhar, loga o erro (em produção, recolocaria na fila).
    /// </summary>
    private async Task ProcessarEvento(EventoPedidoAprovado evento)
    {
        // ╔═══════════════════════════════════════════════════════╗
        // ║  TODO 7: Chamar NotificacaoApi via HTTP              ║
        // ║                                                      ║
        // ║  Passos:                                             ║
        // ║  1. Crie um HttpClient com _httpFactory               ║
        // ║  2. Monte o body com Destinatario, Assunto e Corpo   ║
        // ║  3. Faça POST para "{url}/api/notificacoes"          ║
        // ║  4. Se der erro (catch), logue — não lance exceção   ║
        // ║                                                      ║
        // ║  Dica: use try/catch para não derrubar o background  ║
        // ║  se o NotificacaoApi falhar (lembre: 20% de falha!)  ║
        // ╚═══════════════════════════════════════════════════════╝

        // try
        // {
        //     var urlNotificacao = _config["ServicoNotificacao"];
        //     var client = _httpFactory.CreateClient();
        //     var body = new
        //     {
        //         Destinatario = evento.Cliente,
        //         Assunto = $"Pedido {evento.PedidoId} confirmado!",
        //         Corpo = $"Seu pedido de {evento.Produto} (R$ {evento.Valor}) foi aprovado."
        //     };
        //     var json = JsonSerializer.Serialize(body);
        //     var conteudo = new StringContent(json, Encoding.UTF8, "application/json");
        //     var resposta = await client.PostAsync($"{urlNotificacao}/api/notificacoes", conteudo);
        //
        //     Console.WriteLine(resposta.IsSuccessStatusCode
        //         ? $"📥 ✅ [BACKGROUND] Notificação enviada — Pedido {evento.PedidoId}"
        //         : $"📥 ⚠️ [BACKGROUND] Falha na notificação — Pedido {evento.PedidoId} (status {resposta.StatusCode})");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"📥 ❌ [BACKGROUND] Erro ao notificar — Pedido {evento.PedidoId}: {ex.Message}");
        // }

        // ⬇⬇⬇ REMOVA ESTA LINHA APÓS COMPLETAR O TODO 7 ⬇⬇⬇
        Console.WriteLine($"⚠️  TODO 7 não implementado — evento {evento.PedidoId} não processado");
        // ⬆⬆⬆ REMOVA ESTA LINHA APÓS COMPLETAR O TODO 7 ⬆⬆⬆
    }
}
