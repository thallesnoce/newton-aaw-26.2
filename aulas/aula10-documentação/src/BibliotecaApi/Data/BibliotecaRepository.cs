using BibliotecaApi.Models;

namespace BibliotecaApi.Data;

/// <summary>
/// Repositório em memória para fins didáticos.
/// Em produção, seria substituído por Entity Framework + banco de dados.
/// </summary>
public class BibliotecaRepository
{
    private readonly List<Autor> _autores = new();
    private readonly List<Livro> _livros = new();
    private readonly List<Emprestimo> _emprestimos = new();
    private int _proximoAutorId = 1;
    private int _proximoLivroId = 1;
    private int _proximoEmprestimoId = 1;

    public BibliotecaRepository()
    {
        SeedDados();
    }

    private void SeedDados()
    {
        var autor1 = new Autor { Id = _proximoAutorId++, Nome = "Machado de Assis", Pais = "Brasil", Biografia = "Maior escritor brasileiro." };
        var autor2 = new Autor { Id = _proximoAutorId++, Nome = "Clarice Lispector", Pais = "Brasil", Biografia = "Uma das escritoras mais importantes da literatura brasileira." };
        var autor3 = new Autor { Id = _proximoAutorId++, Nome = "José Saramago", Pais = "Portugal", Biografia = "Nobel de Literatura em 1998." };
        _autores.AddRange(new[] { autor1, autor2, autor3 });

        var livros = new[]
        {
            new Livro { Id = _proximoLivroId++, Titulo = "Dom Casmurro", Isbn = "978-85-359-0277-5", AnoPublicacao = 1899, Genero = "Romance", AutorId = 1 },
            new Livro { Id = _proximoLivroId++, Titulo = "Memórias Póstumas de Brás Cubas", Isbn = "978-85-359-0278-2", AnoPublicacao = 1881, Genero = "Romance", AutorId = 1 },
            new Livro { Id = _proximoLivroId++, Titulo = "A Hora da Estrela", Isbn = "978-85-325-0156-3", AnoPublicacao = 1977, Genero = "Romance", AutorId = 2 },
            new Livro { Id = _proximoLivroId++, Titulo = "Ensaio sobre a Cegueira", Isbn = "978-85-359-0279-9", AnoPublicacao = 1995, Genero = "Romance", AutorId = 3 },
            new Livro { Id = _proximoLivroId++, Titulo = "Quincas Borba", Isbn = "978-85-359-0280-5", AnoPublicacao = 1891, Genero = "Romance", AutorId = 1 },
        };
        _livros.AddRange(livros);
    }

    // --- Autores ---
    public List<Autor> ObterAutores() => _autores.Select(a =>
    {
        a.Livros = _livros.Where(l => l.AutorId == a.Id).ToList();
        return a;
    }).ToList();

    public Autor? ObterAutorPorId(int id)
    {
        var autor = _autores.FirstOrDefault(a => a.Id == id);
        if (autor != null)
            autor.Livros = _livros.Where(l => l.AutorId == id).ToList();
        return autor;
    }

    public Autor CriarAutor(Autor autor)
    {
        autor.Id = _proximoAutorId++;
        _autores.Add(autor);
        return autor;
    }

    public bool AtualizarAutor(int id, Autor autorAtualizado)
    {
        var autor = _autores.FirstOrDefault(a => a.Id == id);
        if (autor == null) return false;
        autor.Nome = autorAtualizado.Nome;
        autor.Pais = autorAtualizado.Pais;
        autor.Biografia = autorAtualizado.Biografia;
        return true;
    }

    public bool RemoverAutor(int id)
    {
        var autor = _autores.FirstOrDefault(a => a.Id == id);
        if (autor == null) return false;
        _autores.Remove(autor);
        return true;
    }

    // --- Livros ---
    public List<Livro> ObterLivros() => _livros.ToList();

    public Livro? ObterLivroPorId(int id) => _livros.FirstOrDefault(l => l.Id == id);

    public List<Livro> BuscarLivros(string? titulo, string? genero, int? autorId)
    {
        var query = _livros.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(l => l.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(genero))
            query = query.Where(l => l.Genero != null && l.Genero.Contains(genero, StringComparison.OrdinalIgnoreCase));
        if (autorId.HasValue)
            query = query.Where(l => l.AutorId == autorId.Value);
        return query.ToList();
    }

    public Livro CriarLivro(Livro livro)
    {
        livro.Id = _proximoLivroId++;
        _livros.Add(livro);
        return livro;
    }

    public bool AtualizarLivro(int id, Livro livroAtualizado)
    {
        var livro = _livros.FirstOrDefault(l => l.Id == id);
        if (livro == null) return false;
        livro.Titulo = livroAtualizado.Titulo;
        livro.Isbn = livroAtualizado.Isbn;
        livro.AnoPublicacao = livroAtualizado.AnoPublicacao;
        livro.Genero = livroAtualizado.Genero;
        livro.AutorId = livroAtualizado.AutorId;
        livro.Disponivel = livroAtualizado.Disponivel;
        return true;
    }

    public bool RemoverLivro(int id)
    {
        var livro = _livros.FirstOrDefault(l => l.Id == id);
        if (livro == null) return false;
        _livros.Remove(livro);
        return true;
    }

    // --- Empréstimos ---
    public List<Emprestimo> ObterEmprestimos() => _emprestimos.ToList();

    public Emprestimo? ObterEmprestimoPorId(int id) => _emprestimos.FirstOrDefault(e => e.Id == id);

    public Emprestimo? CriarEmprestimo(Emprestimo emprestimo)
    {
        var livro = _livros.FirstOrDefault(l => l.Id == emprestimo.LivroId);
        if (livro == null || !livro.Disponivel) return null;

        emprestimo.Id = _proximoEmprestimoId++;
        emprestimo.DataEmprestimo = DateTime.UtcNow;
        emprestimo.DataDevolucaoPrevista = DateTime.UtcNow.AddDays(14);
        emprestimo.Status = StatusEmprestimo.Ativo;
        livro.Disponivel = false;
        _emprestimos.Add(emprestimo);
        return emprestimo;
    }

    public bool DevolverLivro(int emprestimoId)
    {
        var emprestimo = _emprestimos.FirstOrDefault(e => e.Id == emprestimoId);
        if (emprestimo == null || emprestimo.Status == StatusEmprestimo.Devolvido) return false;

        emprestimo.DataDevolucaoEfetiva = DateTime.UtcNow;
        emprestimo.Status = StatusEmprestimo.Devolvido;

        var livro = _livros.FirstOrDefault(l => l.Id == emprestimo.LivroId);
        if (livro != null) livro.Disponivel = true;

        return true;
    }
}
