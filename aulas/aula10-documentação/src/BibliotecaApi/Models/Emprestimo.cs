using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models;

public class Emprestimo
{
    public int Id { get; set; }

    [Required]
    public int LivroId { get; set; }

    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(200)]
    public string NomeUsuario { get; set; } = string.Empty;

    public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;

    public DateTime DataDevolucaoPrevista { get; set; } = DateTime.UtcNow.AddDays(14);

    public DateTime? DataDevolucaoEfetiva { get; set; }

    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Ativo;
}

public enum StatusEmprestimo
{
    Ativo,
    Devolvido,
    Atrasado
}
