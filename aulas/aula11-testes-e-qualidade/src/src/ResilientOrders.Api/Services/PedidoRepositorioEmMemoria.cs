using ResilientOrders.Api.Models;

namespace ResilientOrders.Api.Services;

public class PedidoRepositorioEmMemoria : IPedidoRepositorio
{
    private readonly Dictionary<int, Pedido> _store = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public Pedido? ObterPorId(int id)
    {
        lock (_lock)
        {
            return _store.TryGetValue(id, out var pedido) ? pedido : null;
        }
    }

    public IEnumerable<Pedido> ObterTodos()
    {
        lock (_lock)
        {
            return _store.Values.ToList();
        }
    }

    public Pedido Salvar(Pedido pedido)
    {
        lock (_lock)
        {
            if (pedido.Id == 0)
            {
                pedido.Id = _nextId++;
            }
            _store[pedido.Id] = pedido;
            return pedido;
        }
    }

    public bool Remover(int id)
    {
        lock (_lock)
        {
            return _store.Remove(id);
        }
    }
}
