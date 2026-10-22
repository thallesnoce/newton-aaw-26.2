using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models;

public class Autor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do autor é obrigatório.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Pais { get; set; }

    [StringLength(1000)]
    public string? Biografia { get; set; }

    public List<Livro> Livros { get; set; } = new();
}
