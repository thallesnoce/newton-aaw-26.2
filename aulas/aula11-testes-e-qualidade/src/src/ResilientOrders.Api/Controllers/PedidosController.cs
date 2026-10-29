using Microsoft.AspNetCore.Mvc;
using ResilientOrders.Api.Models;
using ResilientOrders.Api.Services;

namespace ResilientOrders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Pedido>> Listar()
    {
        return Ok(_pedidoService.ListarPedidos());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Pedido> ObterPorId(int id)
    {
        var pedido = _pedidoService.ObterPedido(id);
        if (pedido is null)
        {
            return NotFound(new { message = $"Pedido {id} não encontrado." });
        }
        return Ok(pedido);
    }

    [HttpPost]
    public ActionResult<Pedido> Criar([FromBody] CriarPedidoRequest request)
    {
        if (request is null)
        {
            return BadRequest(new { message = "Payload inválido." });
        }

        try
        {
            var criado = _pedidoService.CriarPedido(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
