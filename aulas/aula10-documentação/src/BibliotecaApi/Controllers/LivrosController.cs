using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly BibliotecaRepository _repo;

    public LivrosController(BibliotecaRepository repo) => _repo = repo;

    [HttpGet]
    public IActionResult ObterTodos(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10)
    {
        tamanhoPagina = Math.Min(tamanhoPagina, 50);
        var todos = _repo.ObterLivros();
        var paginados = todos
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToList();

        var resposta = new RespostaPaginada<Livro>
        {
            Itens = paginados,
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalItens = todos.Count
        };

        return Ok(resposta);
    }

    [HttpGet("{id:int}")]
    public IActionResult ObterPorId(int id)
    {
        var livro = _repo.ObterLivroPorId(id);
        if (livro == null)
            return NotFound(new { mensagem = $"Livro com ID {id} não encontrado." });
        return Ok(livro);
    }

    [HttpGet("buscar")]
    public IActionResult Buscar(
        [FromQuery] string? titulo,
        [FromQuery] string? genero,
        [FromQuery] int? autorId)
    {
        var resultados = _repo.BuscarLivros(titulo, genero, autorId);
        return Ok(resultados);
    }

    [HttpPost]
    public IActionResult Criar([FromBody] Livro livro)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var criado = _repo.CriarLivro(livro);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}")]
    public IActionResult Atualizar(int id, [FromBody] Livro livro)
    {
        if (!_repo.AtualizarLivro(id, livro))
            return NotFound(new { mensagem = $"Livro com ID {id} não encontrado." });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        if (!_repo.RemoverLivro(id))
            return NotFound(new { mensagem = $"Livro com ID {id} não encontrado." });
        return NoContent();
    }
}
