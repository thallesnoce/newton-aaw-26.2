using System.Threading.Channels;
using LojaDev.Compartilhado.Modelos;

namespace LojaDev.Compartilhado.Fila;

/// <summary>
/// Fila em memória baseada em Channel&lt;T&gt; — funciona como um "mini message broker"
/// sem precisar de infraestrutura externa (RabbitMQ, Redis, etc.).
///
/// Channel&lt;T&gt; é thread-safe e suporta múltiplos produtores e consumidores.
/// Na prática real, seria substituído por um broker de mensagens como
/// RabbitMQ, Azure Service Bus, Amazon SQS, etc.
/// </summary>
public class FilaDePedidos
{
    // Canal ilimitado: nunca bloqueia o produtor (o WriteAsync retorna imediatamente).
    // Em produção, usaríamos um canal limitado (BoundedChannel) para backpressure.
    private readonly Channel<EventoPedidoAprovado> _canal =
        Channel.CreateUnbounded<EventoPedidoAprovado>();

    /// <summary>
    /// Publica um evento na fila. Retorna imediatamente (não-bloqueante).
    /// É o equivalente a um "publish" em um broker de mensagens.
    /// </summary>
    public async ValueTask PublicarAsync(EventoPedidoAprovado evento)
    {
        await _canal.Writer.WriteAsync(evento);
        Console.WriteLine($"📤 [FILA] Evento publicado — Pedido {evento.PedidoId}");
    }

    /// <summary>
    /// Lê eventos da fila de forma contínua (bloqueante até ter mensagem).
    /// É o equivalente a um "subscribe/consume" em um broker de mensagens.
    /// </summary>
    public IAsyncEnumerable<EventoPedidoAprovado> ConsumirAsync(
        CancellationToken cancelToken = default)
    {
        return _canal.Reader.ReadAllAsync(cancelToken);
    }
}
