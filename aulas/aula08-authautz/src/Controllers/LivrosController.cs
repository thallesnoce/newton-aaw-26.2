using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

/// <summary>
/// API de Biblioteca da Aula 02 — CRUD completo e funcionando.
/// Hoje ela está TOTALMENTE ABERTA: qualquer um cria, edita e apaga livros.
/// Sua missão nas Etapas 3 e 4 é fechar isso.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly BibliotecaStore _store;

    public LivrosController(BibliotecaStore store) => _store = store;

    // ── Leitura: continua liberada para todos ────────────────────────

    [HttpGet]
    public ActionResult<IEnumerable<Livro>> Listar() => Ok(_store.Listar());

    [HttpGet("{id}")]
    public ActionResult<Livro> BuscarPorId(int id)
    {
        var livro = _store.BuscarPorId(id);
        return livro is null ? NotFound() : Ok(livro);
    }

    // ── Escrita: precisa de token (Etapa 3) ──────────────────────────

    // ╔══════════════════════════════════════════════════════════════╗
    // ║  ETAPA 3 — Proteger os endpoints de escrita                  ║
    // ╚══════════════════════════════════════════════════════════════╝
    // TODO 3.1: adicione o using Microsoft.AspNetCore.Authorization
    //           e o atributo [Authorize] neste método.
    //           Sem token, a resposta deve virar 401 Unauthorized.
    [HttpPost]
    public ActionResult<Livro> Criar([FromBody] LivroRequest dados)
    {
        if (string.IsNullOrWhiteSpace(dados.Titulo))
            return BadRequest(new { message = "Título é obrigatório." });

        var livro = _store.Adicionar(dados);
        return CreatedAtAction(nameof(BuscarPorId), new { id = livro.Id }, livro);
    }

    // TODO 3.2: proteja também o PUT com [Authorize].
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, [FromBody] LivroRequest dados)
    {
        if (string.IsNullOrWhiteSpace(dados.Titulo))
            return BadRequest(new { message = "Título é obrigatório." });

        return _store.Atualizar(id, dados) ? NoContent() : NotFound();
    }

    // ╔══════════════════════════════════════════════════════════════╗
    // ║  ETAPA 4 — Exigir uma role específica                        ║
    // ╚══════════════════════════════════════════════════════════════╝
    // TODO 4.1: remover livro é atribuição do bibliotecário. Proteja este
    //           método com [Authorize(Roles = "Bibliotecario")].
    //           Um token de "Leitor" deve receber 403 Forbidden — repare
    //           que ele ESTÁ autenticado, mas não está autorizado.
    [HttpDelete("{id}")]
    public IActionResult Remover(int id) =>
        _store.Remover(id) ? NoContent() : NotFound();

    // ── Endpoint de apoio para inspecionar o token ───────────────────

    // TODO 3.3 (opcional): proteja com [Authorize] e veja as claims que
    //           chegaram dentro do token — nome, id e role do usuário.
    [HttpGet("/api/quem-sou-eu")]
    public IActionResult QuemSouEu()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Ok(new { autenticado = false, dica = "Faça login e mande o header Authorization: Bearer <token>." });

        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new { autenticado = true, claims });
    }
}
