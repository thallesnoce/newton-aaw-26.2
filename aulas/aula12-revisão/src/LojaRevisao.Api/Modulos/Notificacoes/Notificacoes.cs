using System.Collections.Concurrent;
using System.Threading.Channels;

namespace LojaRevisao.Api.Modulos.Notificacoes;

public record PedidoCriado(int PedidoId);

/// <summary>Provedor de e-mail lento, como os de verdade em dia ruim: leva 3 segundos.</summary>
public class EmailGatewaySimulado
{
    private readonly ConcurrentQueue<int> _enviados = new();

    public IReadOnlyCollection<int> PedidosNotificados => _enviados;

    public async Task EnviarConfirmacaoAsync(int pedidoId)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        _enviados.Enqueue(pedidoId);
    }
}

/// <summary>Fila em memória (já pronta), o mesmo Channel da Aula 06.</summary>
public class FilaDeNotificacoes
{
    private readonly Channel<PedidoCriado> _canal = Channel.CreateUnbounded<PedidoCriado>();

    public ValueTask PublicarAsync(PedidoCriado evento) => _canal.Writer.WriteAsync(evento);

    public IAsyncEnumerable<PedidoCriado> ConsumirAsync(CancellationToken ct) => _canal.Reader.ReadAllAsync(ct);
}

/// <summary>Consumidor em background (já pronto e já registrado): espera eventos que ainda não chegam.</summary>
public class NotificacaoWorker : BackgroundService
{
    private readonly FilaDeNotificacoes _fila;
    private readonly EmailGatewaySimulado _email;

    public NotificacaoWorker(FilaDeNotificacoes fila, EmailGatewaySimulado email)
    {
        _fila = fila;
        _email = email;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var opcoes = new ParallelOptions { MaxDegreeOfParallelism = 20, CancellationToken = stoppingToken };

        await Parallel.ForEachAsync(_fila.ConsumirAsync(stoppingToken), opcoes,
            async (evento, _) => await _email.EnviarConfirmacaoAsync(evento.PedidoId));
    }
}
