namespace ApiVazada.Models;

public class Pedido
{
    public int Id { get; set; }

    /// <summary>Dono do pedido. É este campo que a API esquece de conferir.</summary>
    public int UsuarioId { get; set; }

    public string Descricao { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Pendente";
}
