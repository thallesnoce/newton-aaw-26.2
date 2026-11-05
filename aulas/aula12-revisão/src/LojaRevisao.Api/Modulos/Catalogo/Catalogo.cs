using LojaRevisao.Api.Data;
using LojaRevisao.Api.Models;

namespace LojaRevisao.Api.Modulos.Catalogo;

/// <summary>Regras do Catálogo. Só marca as alterações; quem chama decide quando salvar.</summary>
public class Catalogo : ICatalogo
{
    private readonly LojaDbContext _db;

    public Catalogo(LojaDbContext db) => _db = db;

    public Produto Reservar(int produtoId, int quantidade)
    {
        var produto = _db.Produtos.Find(produtoId)
            ?? throw RegraDeNegocioException.NaoEncontrado($"O produto {produtoId} não existe.");

        if (produto.Estoque < quantidade)
            throw RegraDeNegocioException.Conflito(
                $"Estoque insuficiente de '{produto.Nome}': há {produto.Estoque}, foram pedidas {quantidade}.");

        produto.Estoque -= quantidade;
        return produto;
    }

    public void Devolver(int produtoId, int quantidade)
    {
        var produto = _db.Produtos.Find(produtoId);
        if (produto is not null) produto.Estoque += quantidade;
    }
}
