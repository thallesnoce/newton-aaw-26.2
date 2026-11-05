using System.Security.Claims;
using LojaRevisao.Api.Models;
using LojaRevisao.Api.Modulos;
using LojaRevisao.Api.Modulos.Pedidos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaRevisao.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _service;

    public PedidosController(PedidoService service) => _service = service;

    private int UsuarioLogadoId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Lista os pedidos do usuário logado.</summary>
    [HttpGet]
    public IActionResult Listar() => Ok(_service.ListarDoUsuario(UsuarioLogadoId));

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 09 · AULA 09 — ABAC (não corrija ainda!)
    //  Parte B. A parte A (RBAC) está em ProdutosController.
    //  Especificação: o Cliente vê só os PRÓPRIOS pedidos; o Admin vê todos.
    //  Sintoma: João (id 42) abre o pedido 103, que é da Maria (id 43).
    //  O papel não basta: a decisão depende de um atributo do recurso.
    //  Sua tarefa: comparar o dono do pedido com o usuário do token. Esperado: 403.
    //  Vale também para o cancelamento (Falha 03): ninguém cancela pedido alheio.
    // ══════════════════════════════════════════════════════════════════════
    [HttpGet("{id:int}")]
    public IActionResult Obter(int id)
    {
        var pedido = _service.Obter(id);
        if (pedido is null) return NotFound();
        return Ok(pedido);
    }

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 10 · AULA 10 — Contrato OpenAPI (não corrija ainda!)
    //  Parte A. A parte B está em Program.cs.
    //  Sintoma: o Swagger jura que este POST devolve 200, mas ele devolve 201.
    //  E os erros 400, 401, 404 e 409 não aparecem no contrato.
    //  Sua tarefa: fazer o contrato dizer o que a API realmente faz.
    // ══════════════════════════════════════════════════════════════════════
    [ProducesResponseType(typeof(Pedido), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> Criar(NovoPedido novo)
    {
        try
        {
            var pedido = await _service.CriarAsync(UsuarioLogadoId, novo);
            return CreatedAtAction(nameof(Obter), new { id = pedido.Id }, pedido);
        }
        catch (RegraDeNegocioException ex)
        {
            return Problem(detail: ex.Message, statusCode: ex.Status, title: ex.Titulo);
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 03 · AULA 03 — Design de APIs (não corrija ainda!)
    //  Sintoma: GET /api/v1/cancelarPedido?id=7. Há um verbo na URI, e um GET
    //  que altera dados: um crawler ou um link aberto cancela pedidos.
    //  Sua tarefa: redesenhar para o método HTTP certo, sobre o recurso
    //  certo, com o status code certo (o cancelamento não devolve corpo).
    //  Pedido inexistente continua respondendo 404.
    // ══════════════════════════════════════════════════════════════════════
    [HttpGet("/api/v1/cancelarPedido")]
    public IActionResult Cancelar([FromQuery] int id)
    {
        var pedido = _service.Obter(id);
        if (pedido is null) return NotFound();

        _service.Cancelar(pedido);
        return Ok(new { mensagem = "Pedido cancelado" });
    }
}
