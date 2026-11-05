using LojaRevisao.Api.Models;

namespace LojaRevisao.Api.Data;

public record UsuarioFicticio(int Id, string Nome, string Email, string Senha, string Role);

/// <summary>Dados da prática. O banco é recriado a cada "dotnet run".</summary>
public static class DadosIniciais
{
    // Senhas em texto puro: só porque são usuários fictícios de sala de aula.
    public static readonly IReadOnlyList<UsuarioFicticio> Usuarios = new[]
    {
        new UsuarioFicticio(1,  "Ana",   "ana@loja.com",   "admin123", "Admin"),
        new UsuarioFicticio(42, "João",  "joao@loja.com",  "joao123",  "Cliente"),
        new UsuarioFicticio(43, "Maria", "maria@loja.com", "maria123", "Cliente"),
    };

    public static void Popular(LojaDbContext db)
    {
        db.Produtos.AddRange(
            new Produto { Id = 1, Nome = "Teclado Mecânico", Preco = 150m, Estoque = 1000 },
            new Produto { Id = 2, Nome = "Mouse Sem Fio", Preco = 80m, Estoque = 1000 },
            new Produto { Id = 3, Nome = "Cadeira de Escritório", Preco = 300m, Estoque = 1000 },
            new Produto { Id = 4, Nome = "Monitor 27 Polegadas", Preco = 900m, Estoque = 1000 });

        // Catálogo grande de propósito: 2.000 produtos no total.
        for (var id = 5; id <= 2000; id++)
            db.Produtos.Add(new Produto { Id = id, Nome = $"Acessório {id:0000}", Preco = 10m + id % 90, Estoque = 100 });

        db.Pedidos.AddRange(
            new Pedido
            {
                Id = 101, UsuarioId = 42, Subtotal = 150m, Frete = 25m, Total = 175m,
                Itens = { new ItemPedido { ProdutoId = 1, Quantidade = 1, PrecoUnitario = 150m } }
            },
            new Pedido
            {
                Id = 103, UsuarioId = 43, Subtotal = 160m, Frete = 25m, Total = 185m,
                Itens = { new ItemPedido { ProdutoId = 2, Quantidade = 2, PrecoUnitario = 80m } }
            });

        db.SaveChanges();
    }
}
