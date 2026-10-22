using BibliotecaApi.Models;

namespace BibliotecaApi.Data;

/// <summary>
/// Armazenamento em memória — sem banco de dados, para a aula focar em
/// autorização. Os dados voltam ao estado inicial a cada reinício da API.
/// </summary>
public class BibliotecaStore
{
    private readonly List<Livro> _livros = new()
    {
        new Livro { Id = 1, Titulo = "Criando Microsserviços", Autor = "Sam Newman", Ano = 2022 },
        new Livro { Id = 2, Titulo = "Release It!", Autor = "Michael Nygard", Ano = 2018 },
        new Livro { Id = 3, Titulo = "Clean Architecture", Autor = "Robert C. Martin", Ano = 2017 },
        new Livro { Id = 4, Titulo = "Os Lusíadas (edição fac-símile de 1572)", Autor = "Luís de Camões", Ano = 1572, Categoria = "ObraRara" }
    };

    private readonly List<Emprestimo> _emprestimos = new()
    {
        new Emprestimo { Id = 101, LivroId = 1, UsuarioId = 1, Devolucao = DateOnly.FromDateTime(DateTime.Today.AddDays(7)) },  // da Ana
        new Emprestimo { Id = 102, LivroId = 2, UsuarioId = 2, Devolucao = DateOnly.FromDateTime(DateTime.Today.AddDays(3)) }   // do Bruno
    };

    private int _proximoLivroId = 5;
    private int _proximoEmprestimoId = 103;

    /// <summary>
    /// Usuários fictícios — um por papel da matriz (o README tem a matriz completa).
    /// </summary>
    public static readonly Usuario[] Usuarios =
    {
        new Usuario(1, "Ana Leitora", "ana@biblioteca.com", "ana123", "Leitor", "(31) 90000-0001", "000.000.001-91"),
        new Usuario(2, "Bruno Leitor", "bruno@biblioteca.com", "bruno123", "Leitor", "(31) 90000-0002", "000.000.002-72"),
        new Usuario(3, "Carla Pesquisadora", "carla@biblioteca.com", "carla123", "Pesquisador", "(31) 90000-0003", "000.000.003-53"),
        new Usuario(4, "Beto Bibliotecário", "biblio@biblioteca.com", "biblio123", "Bibliotecario", "(31) 90000-0004", "000.000.004-34"),
        new Usuario(5, "Dora Administradora", "admin@biblioteca.com", "admin123", "Admin", "(31) 90000-0005", "000.000.005-15")
    };

    public static Usuario? Autenticar(string email, string senha) =>
        Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);

    // ── Livros ───────────────────────────────────────────────────────

    public IReadOnlyList<Livro> ListarLivros() => _livros;

    public Livro? BuscarLivro(int id) => _livros.FirstOrDefault(l => l.Id == id);

    public Livro AdicionarLivro(LivroRequest dados)
    {
        var livro = new Livro
        {
            Id = _proximoLivroId++,
            Titulo = dados.Titulo,
            Autor = dados.Autor,
            Ano = dados.Ano,
            Disponivel = dados.Disponivel,
            Categoria = dados.Categoria
        };
        _livros.Add(livro);
        return livro;
    }

    public bool AtualizarLivro(int id, LivroRequest dados)
    {
        var livro = BuscarLivro(id);
        if (livro is null) return false;

        livro.Titulo = dados.Titulo;
        livro.Autor = dados.Autor;
        livro.Ano = dados.Ano;
        livro.Disponivel = dados.Disponivel;
        livro.Categoria = dados.Categoria;
        return true;
    }

    public bool RemoverLivro(int id)
    {
        var livro = BuscarLivro(id);
        if (livro is null) return false;
        _livros.Remove(livro);
        return true;
    }

    // ── Empréstimos ──────────────────────────────────────────────────

    public IReadOnlyList<Emprestimo> ListarEmprestimos() => _emprestimos;

    public Emprestimo? BuscarEmprestimo(int id) => _emprestimos.FirstOrDefault(e => e.Id == id);

    public Emprestimo CriarEmprestimo(int livroId, int usuarioId)
    {
        var emprestimo = new Emprestimo
        {
            Id = _proximoEmprestimoId++,
            LivroId = livroId,
            UsuarioId = usuarioId,
            Devolucao = DateOnly.FromDateTime(DateTime.Today.AddDays(14))
        };
        _emprestimos.Add(emprestimo);
        return emprestimo;
    }

    public void Renovar(Emprestimo emprestimo)
    {
        emprestimo.Devolucao = emprestimo.Devolucao.AddDays(7);
        emprestimo.Renovacoes++;
    }
}
