namespace ResilientOrders.Api.Models;

public class Pedido
{
    public int Id { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public List<ItemPedido> Itens { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal Frete { get; set; }
    public decimal Total { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}

public class ItemPedido
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}

public record CriarPedidoRequest(string NomeCliente, List<ItemPedido> Itens);
