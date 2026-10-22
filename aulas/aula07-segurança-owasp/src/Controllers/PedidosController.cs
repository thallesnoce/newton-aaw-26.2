using ApiVazada.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiVazada.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _db;

    public PedidosController(AppDbContext db) => _db = db;

    // ══════════════════════════════════════════════════════════════════
    //  FALHA 2 (não corrija ainda!) — IDOR / Broken Access Control
    //  O endpoint exige um token válido (autenticação), mas nunca compara
    //  o dono do pedido com o usuário do token (autorização).
    //  Estar logado não é o mesmo que ter direito àquele recurso.
    //  Teste: logue como joao@loja.com (id 42) e peça GET /api/pedidos/103,
    //         que pertence à Maria (id 43).
    // ══════════════════════════════════════════════════════════════════
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var pedido = _db.Pedidos.Find(id);
        if (pedido is null) return NotFound();
        return Ok(pedido);
    }

    /// <summary>Lista os pedidos de um usuário — mesma falha, na listagem.</summary>
    [Authorize]
    [HttpGet("usuario/{usuarioId}")]
    public IActionResult GetByUsuario(int usuarioId)
    {
        var pedidos = _db.Pedidos.Where(p => p.UsuarioId == usuarioId).ToList();
        return Ok(pedidos);
    }
}
