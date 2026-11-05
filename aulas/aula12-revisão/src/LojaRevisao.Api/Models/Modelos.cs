using System.ComponentModel.DataAnnotations;

namespace LojaRevisao.Api.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}

public class Pedido
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Frete { get; set; }
    public decimal Total { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
}

public class ItemPedido
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}

public record NovoProduto(
    [Required, StringLength(120)] string Nome,
    [Range(0.01, 1000000)] decimal Preco,
    [Range(0, 1000000)] int Estoque);

public record NovoItem(
    [Range(1, int.MaxValue)] int ProdutoId,
    [Range(1, int.MaxValue)] int Quantidade);

public record NovoPedido([Required, MinLength(1)] List<NovoItem> Itens);

public record Credenciais([Required] string Email, [Required] string Senha);

/// <summary>Envelope de paginação (já pronto, é só usar).</summary>
public record Pagina<T>(int Page, int Size, int Total, List<T> Items);
