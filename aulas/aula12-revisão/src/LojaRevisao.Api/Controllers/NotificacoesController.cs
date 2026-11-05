using LojaRevisao.Api.Modulos.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaRevisao.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/notificacoes")]
public class NotificacoesController : ControllerBase
{
    /// <summary>Ids dos pedidos cuja confirmação já foi enviada por e-mail.</summary>
    [HttpGet]
    public IActionResult Listar([FromServices] EmailGatewaySimulado email) => Ok(email.PedidosNotificados);
}
