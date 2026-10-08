namespace BibliotecaApi.Models;

/// <summary>
/// Mesmo modelo da API de Biblioteca construída na Aula 02.
/// </summary>
public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int Ano { get; set; }
    public bool Disponivel { get; set; } = true;
}

/// <summary>Corpo aceito no POST e no PUT (o cliente não escolhe o Id).</summary>
public record LivroRequest(string Titulo, string Autor, int Ano, bool Disponivel);
