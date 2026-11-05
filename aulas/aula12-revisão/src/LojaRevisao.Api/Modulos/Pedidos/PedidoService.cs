using LojaRevisao.Api.Data;
using LojaRevisao.Api.Models;
using LojaRevisao.Api.Modulos.Catalogo;
using LojaRevisao.Api.Modulos.Notificacoes;
using Microsoft.EntityFrameworkCore;

namespace LojaRevisao.Api.Modulos.Pedidos;

public class PedidoService
{
    private readonly LojaDbContext _db;
    private readonly ICatalogo _catalogo;
    private readonly EmailGatewaySimulado _email;
    private readonly FilaDeNotificacoes _fila;

    public PedidoService(LojaDbContext db, ICatalogo catalogo, EmailGatewaySimulado email, FilaDeNotificacoes fila)
    {
        _db = db;
        _catalogo = catalogo;
        _email = email;
        _fila = fila;
    }

    public async Task<Pedido> CriarAsync(int usuarioId, NovoPedido novo)
    {
        var pedido = new Pedido { UsuarioId = usuarioId };

        foreach (var item in novo.Itens)
        {
            // ══════════════════════════════════════════════════════════════
            //  FALHA 01 · AULA 01 — Monólito e fronteiras (não corrija ainda!)
            //  Sintoma: comprar 999.999 unidades funciona e o estoque fica
            //  NEGATIVO. O módulo de Pedidos mexe direto nos dados do módulo
            //  de Catálogo e pula a regra que mora lá.
            //  Pense: e no dia em que o Catálogo virar o ServicoProdutos?
            //  Sua tarefa: fazer Pedidos falar com o Catálogo só pelo
            //  contrato dele (leia Modulos/Catalogo/ICatalogo.cs).
            // ══════════════════════════════════════════════════════════════
            var produto = _db.Produtos.Find(item.ProdutoId)
                ?? throw RegraDeNegocioException.NaoEncontrado($"O produto {item.ProdutoId} não existe.");
            produto.Estoque -= item.Quantidade;

            // ══════════════════════════════════════════════════════════════
            //  FALHA 05 · AULA 05 — Persistência e ACID (não corrija ainda!)
            //  Sintoma: num pedido com um item válido e outro inexistente, a
            //  API responde 404, mas o estoque do item válido já baixou.
            //  Nenhum pedido foi criado e a mercadoria "sumiu".
            //  Sua tarefa: garantir que o pedido seja TUDO OU NADA.
            // ══════════════════════════════════════════════════════════════
            _db.SaveChanges();

            pedido.Itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                Quantidade = item.Quantidade,
                PrecoUnitario = produto.Preco
            });
        }

        pedido.Subtotal = pedido.Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
        pedido.Frete = CalculadoraDePedido.CalcularFrete(pedido.Subtotal);
        pedido.Total = CalculadoraDePedido.CalcularTotal(pedido.Subtotal);

        _db.Pedidos.Add(pedido);
        _db.SaveChanges();

        // ══════════════════════════════════════════════════════════════════
        //  FALHA 06 · AULA 06 — Comunicação entre serviços (não corrija ainda!)
        //  Sintoma: todo pedido demora mais de 3 segundos, porque o cliente
        //  fica esperando o provedor de e-mail responder (acoplamento temporal).
        //  Sua tarefa: o e-mail pode chegar depois. Publique um evento
        //  PedidoCriado na fila; o consumidor já existe e já está registrado
        //  (leia Modulos/Notificacoes/Notificacoes.cs).
        // ══════════════════════════════════════════════════════════════════
        await _email.EnviarConfirmacaoAsync(pedido.Id);

        return pedido;
    }

    public Pedido? Obter(int id) =>
        _db.Pedidos.Include(p => p.Itens).FirstOrDefault(p => p.Id == id);

    public List<Pedido> ListarDoUsuario(int usuarioId) =>
        _db.Pedidos.Include(p => p.Itens).Where(p => p.UsuarioId == usuarioId).OrderBy(p => p.Id).ToList();

    /// <summary>Cancela o pedido e devolve as unidades ao estoque.</summary>
    public void Cancelar(Pedido pedido)
    {
        foreach (var item in pedido.Itens)
            _catalogo.Devolver(item.ProdutoId, item.Quantidade);

        _db.Pedidos.Remove(pedido);
        _db.SaveChanges();
    }
}
