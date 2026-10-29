using ResilientOrders.Api.Models;

namespace ResilientOrders.Api.Services;

/// <summary>
/// Regras de negócio do pedido (as mesmas do handout da aula):
///  - Item com quantidade menor ou igual a zero torna o pedido inválido.
///  - Desconto progressivo sobre o subtotal: 5% a partir de R$200 (inclusive)
///    e 10% a partir de R$500 (inclusive).
///  - Frete fixo de R$25; grátis a partir de R$300 (inclusive).
///  - Total = subtotal - desconto + frete.
/// </summary>
public class PedidoService : IPedidoService
{
    public const decimal FretePadrao = 25m;

    private readonly IPedidoRepositorio _repositorio;

    public PedidoService(IPedidoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public decimal CalcularDesconto(decimal subtotal)
    {
        if (subtotal < 0)
        {
            throw new ArgumentException("Subtotal não pode ser negativo.", nameof(subtotal));
        }

        if (subtotal > 500m)
        {
            return Math.Round(subtotal * 0.10m, 2);
        }

        if (subtotal >= 200m)
        {
            return Math.Round(subtotal * 0.05m, 2);
        }

        return 0m;
    }

    public decimal CalcularFrete(decimal subtotal)
    {
        if (subtotal < 0)
        {
            throw new ArgumentException("Subtotal não pode ser negativo.", nameof(subtotal));
        }

        return subtotal >= 300m ? 0m : FretePadrao;
    }

    public decimal CalcularTotal(decimal subtotal)
    {
        return subtotal - CalcularDesconto(subtotal) + CalcularFrete(subtotal);
    }

    public Pedido CriarPedido(CriarPedidoRequest request)
    {
        if (request.Itens is null || request.Itens.Count == 0)
        {
            throw new InvalidOperationException("Pedido não pode estar vazio.");
        }

        if (request.Itens.Any(item => item.Quantidade <= 0))
        {
            throw new ArgumentException("Quantidade de cada item deve ser maior que zero.");
        }

        var subtotal = request.Itens.Sum(item => item.Subtotal);

        var pedido = new Pedido
        {
            NomeCliente = request.NomeCliente,
            Itens = request.Itens,
            Subtotal = subtotal,
            Desconto = CalcularDesconto(subtotal),
            Frete = CalcularFrete(subtotal),
            Total = CalcularTotal(subtotal)
        };

        return _repositorio.Salvar(pedido);
    }

    public Pedido? ObterPedido(int id) => _repositorio.ObterPorId(id);

    public IEnumerable<Pedido> ListarPedidos() => _repositorio.ObterTodos();
}
