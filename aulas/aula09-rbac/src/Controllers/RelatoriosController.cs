using BibliotecaApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

/// <summary>
/// Relatórios gerenciais — criados às pressas por um estagiário na sexta-feira.
/// Pela matriz do README, são exclusivos do papel Admin: expõem DADOS PESSOAIS
/// (telefone, CPF) protegidos pela LGPD.
///
/// FALHA 2 (não corrija ainda!) — compare com o comentário em Program.cs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly BibliotecaStore _store;

    public RelatoriosController(BibliotecaStore store) => _store = store;

    /// <summary>GET /api/relatorios/leitores — cadastro completo dos usuários.</summary>
    [HttpGet("leitores")]
    public IActionResult Leitores() =>
        Ok(BibliotecaStore.Usuarios.Select(u => new { u.Id, u.Nome, u.Email, u.Telefone, u.Cpf, u.Role }));

    /// <summary>GET /api/relatorios/emprestimos — quem está com qual livro.</summary>
    [HttpGet("emprestimos")]
    public IActionResult Emprestimos() =>
        Ok(_store.ListarEmprestimos().Select(e => new
        {
            e.Id,
            Livro = _store.BuscarLivro(e.LivroId)?.Titulo,
            Usuario = BibliotecaStore.Usuarios.First(u => u.Id == e.UsuarioId).Nome,
            e.Devolucao
        }));
}
