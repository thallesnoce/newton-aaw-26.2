using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly BibliotecaRepository _repo;

    public AutoresController(BibliotecaRepository repo) => _repo = repo;

    [HttpGet]
    public IActionResult ObterTodos()
    {
        return Ok(_repo.ObterAutores());
    }

    [HttpGet("{id:int}")]
    public IActionResult ObterPorId(int id)
    {
        var autor = _repo.ObterAutorPorId(id);
        if (autor == null)
            return NotFound(new { mensagem = $"Autor com ID {id} não encontrado." });
        return Ok(autor);
    }

    [HttpPost]
    public IActionResult Criar([FromBody] Autor autor)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var criado = _repo.CriarAutor(autor);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}")]
    public IActionResult Atualizar(int id, [FromBody] Autor autor)
    {
        if (!_repo.AtualizarAutor(id, autor))
            return NotFound(new { mensagem = $"Autor com ID {id} não encontrado." });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        if (!_repo.RemoverAutor(id))
            return NotFound(new { mensagem = $"Autor com ID {id} não encontrado." });
        return NoContent();
    }
}
