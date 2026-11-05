using LojaRevisao.Api.Models;

namespace LojaRevisao.Api.Modulos.Catalogo;

/// <summary>
/// O contrato do módulo de Catálogo: é a ÚNICA porta de entrada para quem está fora dele.
/// Quando o módulo virar o ServicoProdutos, esta interface vira chamadas HTTP.
/// </summary>
public interface ICatalogo
{
    /// <summary>Baixa o estoque, ou lança 404 (produto inexistente) ou 409 (estoque insuficiente).</summary>
    Produto Reservar(int produtoId, int quantidade);

    /// <summary>Devolve ao estoque as unidades de um pedido cancelado.</summary>
    void Devolver(int produtoId, int quantidade);
}
