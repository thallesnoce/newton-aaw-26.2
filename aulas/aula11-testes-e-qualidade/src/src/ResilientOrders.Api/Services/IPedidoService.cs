using ResilientOrders.Api.Models;

namespace ResilientOrders.Api.Services;

public interface IPedidoService
{
    decimal CalcularDesconto(decimal subtotal);
    decimal CalcularFrete(decimal subtotal);
    decimal CalcularTotal(decimal subtotal);
    Pedido CriarPedido(CriarPedidoRequest request);
    Pedido? ObterPedido(int id);
    IEnumerable<Pedido> ListarPedidos();
}
