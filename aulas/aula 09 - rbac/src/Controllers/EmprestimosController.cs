using System.Security.Claims;
using BibliotecaApi.Autorizacao;
using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

/// <summary>
/// Empréstimos — aqui o PAPEL sozinho não basta para decidir.
/// Este controller está CORRETO: ele é o ponto de aplicação (PEP) e
/// pergunta a quem decide. Os erros estão em quem decide.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmprestimosController : ControllerBase
{
    private readonly BibliotecaStore _store;
    private readonly IAuthorizationService _autorizacao;
    private readonly MotorDePoliticas _motor;

    public EmprestimosController(BibliotecaStore store, IAuthorizationService autorizacao, MotorDePoliticas motor)
    {
        _store = store;
        _autorizacao = autorizacao;
        _motor = motor;
    }

    /// <summary>
    /// POST /api/emprestimos — PBAC: a decisão vem do documento de política
    /// (politicas.json), avaliado pelo MotorDePoliticas. → FALHA 4
    /// </summary>
    [HttpPost]
    public IActionResult Emprestar([FromBody] EmprestimoRequest dados)
    {
        var livro = _store.BuscarLivro(dados.LivroId);
        if (livro is null) return NotFound();

        var decisao = _motor.Avaliar(User, "emprestar", livro);
        if (!decisao.Permitido)
            return StatusCode(StatusCodes.Status403Forbidden,
                new { message = "Negado pela política.", regra = decisao.Regra });

        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var emprestimo = _store.CriarEmprestimo(livro.Id, usuarioId);
        return Created($"/api/emprestimos/{emprestimo.Id}", new { emprestimo, regra = decisao.Regra });
    }

    /// <summary>
    /// POST /api/emprestimos/{id}/renovar — ABAC: a policy recebe o
    /// RECURSO (o empréstimo) e decide olhando os atributos dele. → FALHA 3
    /// </summary>
    [HttpPost("{id}/renovar")]
    public async Task<IActionResult> Renovar(int id)
    {
        var emprestimo = _store.BuscarEmprestimo(id);
        if (emprestimo is null) return NotFound();

        var resultado = await _autorizacao.AuthorizeAsync(User, emprestimo, "PodeRenovarEmprestimo");
        if (!resultado.Succeeded) return Forbid();

        _store.Renovar(emprestimo);
        return Ok(emprestimo);
    }
}
