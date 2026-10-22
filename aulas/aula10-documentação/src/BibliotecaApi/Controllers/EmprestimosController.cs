using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

// Endpoints de empréstimo exigem login (JWT da Aula 07, pré-pronto).
// É por causa deles que o Swagger precisará do security scheme (Passo 9):
// sem o botão Authorize, o "Try it out" daqui devolve 401.
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmprestimosController : ControllerBase
{
    private readonly BibliotecaRepository _repo;

    public EmprestimosController(BibliotecaRepository repo) => _repo = repo;

    [HttpGet]
    public IActionResult ObterTodos()
    {
        return Ok(_repo.ObterEmprestimos());
    }

    [HttpGet("{id:int}")]
    public IActionResult ObterPorId(int id)
    {
        var emprestimo = _repo.ObterEmprestimoPorId(id);
        if (emprestimo == null)
            return NotFound(new { mensagem = $"Empréstimo com ID {id} não encontrado." });
        return Ok(emprestimo);
    }

    [HttpPost]
    public IActionResult Criar([FromBody] Emprestimo emprestimo)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var criado = _repo.CriarEmprestimo(emprestimo);
        if (criado == null)
            return Conflict(new { mensagem = "Livro não encontrado ou não está disponível para empréstimo." });

        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
    }

    [HttpPatch("{id:int}/devolver")]
    public IActionResult Devolver(int id)
    {
        if (!_repo.DevolverLivro(id))
            return NotFound(new { mensagem = "Empréstimo não encontrado ou já devolvido." });

        var emprestimo = _repo.ObterEmprestimoPorId(id);
        return Ok(new { mensagem = "Livro devolvido com sucesso.", emprestimo });
    }
}
