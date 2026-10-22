using ApiVazada.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVazada.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Dados fictícios — loja de brinquedo, nenhum dado real.
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario { Id = 1, Nome = "Ana Admin", Email = "ana@loja.com", Senha = "admin123", Role = "Admin" },
            new Usuario { Id = 42, Nome = "João Silva", Email = "joao@loja.com", Senha = "joao123", Role = "User" },
            new Usuario { Id = 43, Nome = "Maria Souza", Email = "maria@loja.com", Senha = "maria123", Role = "User" }
        );

        modelBuilder.Entity<Produto>().HasData(
            new Produto { Id = 1, Nome = "Notebook Gamer", Descricao = "Notebook para jogos", Preco = 5000m },
            new Produto { Id = 2, Nome = "Smartphone", Descricao = "Celular moderno", Preco = 2500m },
            new Produto { Id = 3, Nome = "Teclado Mecânico", Descricao = "Teclado com switches azuis", Preco = 350m }
        );

        modelBuilder.Entity<Pedido>().HasData(
            new Pedido { Id = 101, UsuarioId = 42, Descricao = "Notebook Gamer", ValorTotal = 5000m, Status = "Entregue" },
            new Pedido { Id = 102, UsuarioId = 42, Descricao = "Teclado Mecânico", ValorTotal = 350m, Status = "Pendente" },
            new Pedido { Id = 103, UsuarioId = 43, Descricao = "Smartphone", ValorTotal = 2500m, Status = "Processando" }
        );
    }
}
