namespace BibliotecaApi.Models;

/// <summary>
/// Mesmo modelo das Aulas 02 e 08, agora com um ATRIBUTO DO RECURSO:
/// a categoria. É ela que a política PBAC (Falha 4) usa para decidir.
/// </summary>
public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int Ano { get; set; }
    public bool Disponivel { get; set; } = true;

    /// <summary>"Comum" ou "ObraRara".</summary>
    public string Categoria { get; set; } = "Comum";
}

/// <summary>Corpo aceito no POST e no PUT (o cliente não escolhe o Id).</summary>
public record LivroRequest(string Titulo, string Autor, int Ano, bool Disponivel, string Categoria = "Comum");
