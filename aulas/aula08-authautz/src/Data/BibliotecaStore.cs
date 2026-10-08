using BibliotecaApi.Models;

namespace BibliotecaApi.Data;

/// <summary>
/// Armazenamento em memória — sem banco de dados, para a aula focar em
/// autenticação e autorização. Os dados voltam ao estado inicial a cada
/// reinício da API.
/// </summary>
public class BibliotecaStore
{
    private readonly List<Livro> _livros =
    [
        new Livro { Id = 1, Titulo = "Criando Microsserviços", Autor = "Sam Newman", Ano = 2022, Disponivel = true },
        new Livro { Id = 2, Titulo = "Release It!", Autor = "Michael Nygard", Ano = 2018, Disponivel = true },
        new Livro { Id = 3, Titulo = "Clean Architecture", Autor = "Robert C. Martin", Ano = 2017, Disponivel = false }
    ];

    private int _proximoId = 4;

    /// <summary>
    /// Usuários fictícios. As roles são as que a API vai exigir:
    /// "Leitor" pode consultar; "Bibliotecario" pode remover.
    /// </summary>
    public static readonly Usuario[] Usuarios =
    [
        new Usuario(1, "Lia Leitora", "leitor@biblioteca.com", "leitor123", "Leitor"),
        new Usuario(2, "Beto Bibliotecário", "biblio@biblioteca.com", "biblio123", "Bibliotecario")
    ];

    public IReadOnlyList<Livro> Listar() => _livros;

    public Livro? BuscarPorId(int id) => _livros.FirstOrDefault(l => l.Id == id);

    public Livro Adicionar(LivroRequest dados)
    {
        var livro = new Livro
        {
            Id = _proximoId++,
            Titulo = dados.Titulo,
            Autor = dados.Autor,
            Ano = dados.Ano,
            Disponivel = dados.Disponivel
        };
        _livros.Add(livro);
        return livro;
    }

    public bool Atualizar(int id, LivroRequest dados)
    {
        var livro = BuscarPorId(id);
        if (livro is null) return false;

        livro.Titulo = dados.Titulo;
        livro.Autor = dados.Autor;
        livro.Ano = dados.Ano;
        livro.Disponivel = dados.Disponivel;
        return true;
    }

    public bool Remover(int id)
    {
        var livro = BuscarPorId(id);
        if (livro is null) return false;
        _livros.Remove(livro);
        return true;
    }

    public static Usuario? Autenticar(string email, string senha) =>
        Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
}
