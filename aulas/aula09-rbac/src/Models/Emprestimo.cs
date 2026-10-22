namespace BibliotecaApi.Models;

/// <summary>
/// Empréstimo de um livro. O atributo que importa para a autorização
/// é <see cref="UsuarioId"/>: o DONO do empréstimo (Falha 3 — ABAC).
/// </summary>
public class Emprestimo
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public int UsuarioId { get; set; }
    public DateOnly Devolucao { get; set; }
    public int Renovacoes { get; set; }
}

/// <summary>Corpo do POST /api/emprestimos.</summary>
public record EmprestimoRequest(int LivroId);
