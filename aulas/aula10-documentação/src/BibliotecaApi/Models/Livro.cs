using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models;

public class Livro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(300, MinimumLength = 1)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Isbn { get; set; }

    [Range(1000, 2100, ErrorMessage = "Ano de publicação inválido.")]
    public int AnoPublicacao { get; set; }

    [StringLength(100)]
    public string? Genero { get; set; }

    [Required(ErrorMessage = "O autor é obrigatório.")]
    public int AutorId { get; set; }

    public bool Disponivel { get; set; } = true;
}
