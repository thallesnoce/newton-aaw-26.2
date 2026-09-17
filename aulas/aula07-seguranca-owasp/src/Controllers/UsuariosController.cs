using ApiVazada.Data;
using ApiVazada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiVazada.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsuariosController(AppDbContext db) => _db = db;

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var usuario = _db.Usuarios.Find(id);
        if (usuario is null) return NotFound();
        return Ok(new { usuario.Id, usuario.Nome, usuario.Email, usuario.Role });
    }

    // ══════════════════════════════════════════════════════════════════
    //  FALHA 4 (não corrija ainda!) — Mass Assignment
    //  O método recebe a ENTIDADE inteira vinda do JSON. O cliente deveria
    //  poder editar apenas nome e e-mail, mas qualquer campo do modelo
    //  entra junto — inclusive Role e Senha.
    //  Teste: PUT /api/usuarios/42 com o corpo
    //         { "nome": "João", "email": "joao@loja.com", "role": "Admin" }
    //         Faça login de novo e veja a role no token.
    // ══════════════════════════════════════════════════════════════════
    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, [FromBody] Usuario dados)
    {
        var usuario = _db.Usuarios.Find(id);
        if (usuario is null) return NotFound();

        usuario.Nome = dados.Nome;
        usuario.Email = dados.Email;
        usuario.Role = dados.Role;

        _db.SaveChanges();
        return Ok(new { usuario.Id, usuario.Nome, usuario.Email, usuario.Role });
    }
}
