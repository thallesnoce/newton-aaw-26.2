using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

/// <summary>
/// API de Biblioteca como terminou a Aula 08: já exige token na escrita.
/// O problema agora não é "quem é você" (401), é "o que você pode" (403).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly BibliotecaStore _store;

    public LivrosController(BibliotecaStore store) => _store = store;

    // ── Leitura: pública DE PROPÓSITO (consultar o acervo não exige login) ──

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<IEnumerable<Livro>> Listar() => Ok(_store.ListarLivros());

    [AllowAnonymous]
    [HttpGet("{id}")]
    public ActionResult<Livro> BuscarPorId(int id)
    {
        var livro = _store.BuscarLivro(id);
        return livro is null ? NotFound() : Ok(livro);
    }

    // ┌──────────────────────────────────────────────────────────────────┐
    // │ FALHA 1 (não corrija ainda!)                                      │
    // │ Os atributos abaixo NÃO batem com a matriz papel × permissão do   │
    // │ README. Compare linha a linha: quem pode cadastrar? editar?       │
    // │ remover?                                                          │
    // └──────────────────────────────────────────────────────────────────┘

    [Authorize]
    [HttpPost]
    public ActionResult<Livro> Criar([FromBody] LivroRequest dados)
    {
        if (string.IsNullOrWhiteSpace(dados.Titulo))
            return BadRequest(new { message = "Título é obrigatório." });

        var livro = _store.AdicionarLivro(dados);
        return CreatedAtAction(nameof(BuscarPorId), new { id = livro.Id }, livro);
    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, [FromBody] LivroRequest dados)
    {
        if (string.IsNullOrWhiteSpace(dados.Titulo))
            return BadRequest(new { message = "Título é obrigatório." });

        return _store.AtualizarLivro(id, dados) ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Bibliotecario,Admin")]
    [HttpDelete("{id}")]
    public IActionResult Remover(int id) =>
        _store.RemoverLivro(id) ? NoContent() : NotFound();
}
