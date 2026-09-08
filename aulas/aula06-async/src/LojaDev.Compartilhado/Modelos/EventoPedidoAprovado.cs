namespace LojaDev.Compartilhado.Modelos;

/// <summary>
/// Evento que representa um pedido aprovado, publicado na fila
/// para processamento assíncrono.
/// </summary>
public class EventoPedidoAprovado
{
    public Guid PedidoId { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Produto { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime AprovadoEm { get; set; } = DateTime.Now;
}
