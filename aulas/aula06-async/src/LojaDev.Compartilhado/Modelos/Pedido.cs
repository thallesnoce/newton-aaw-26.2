namespace LojaDev.Compartilhado.Modelos;

/// <summary>
/// Representa um pedido feito pelo cliente.
/// </summary>
public class Pedido
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Cliente { get; set; } = string.Empty;
    public string Produto { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
}
