using ResilientOrders.Api.Models;

namespace ResilientOrders.Api.Services;

public interface IPedidoRepositorio
{
    Pedido? ObterPorId(int id);
    IEnumerable<Pedido> ObterTodos();
    Pedido Salvar(Pedido pedido);
    bool Remover(int id);
}
