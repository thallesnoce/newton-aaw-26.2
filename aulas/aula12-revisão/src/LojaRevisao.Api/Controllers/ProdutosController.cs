using LojaRevisao.Api.Data;
using LojaRevisao.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaRevisao.Api.Controllers;

[ApiController]
[Route("api/v1/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly LojaDbContext _db;

    public ProdutosController(LojaDbContext db) => _db = db;

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 04 · AULA 04 — Revisão de REST: paginação (não corrija ainda!)
    //  Sintoma: GET /api/v1/produtos devolve os 2.000 produtos, sempre.
    //  Sua tarefa: aceitar ?page=1&size=10 e responder no envelope Pagina<T>
    //  (já existe em Models/Modelos.cs). Use valores padrão, ordene por Id e
    //  imponha no servidor um teto de 100 itens por página.
    // ══════════════════════════════════════════════════════════════════════
    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_db.Produtos.ToList());
    }

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 07 · AULA 07 — Segurança, OWASP Injection (não corrija ainda!)
    //  Sintoma: GET /api/v1/produtos/busca?nome=%' OR '1'='1 ignora o filtro.
    //  O texto do usuário vira parte do comando SQL.
    //  Sua tarefa: separar COMANDO de DADO.
    // ══════════════════════════════════════════════════════════════════════
    [HttpGet("busca")]
    public IActionResult Buscar([FromQuery] string nome)
    {
        var sql = "SELECT * FROM Produtos WHERE Nome LIKE '%" + nome + "%'";
        return Ok(_db.Produtos.FromSqlRaw(sql).Take(50).ToList());
    }

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 02 · AULA 02 — Fundamentos de REST: status code (não corrija ainda!)
    //  Parte A — Sintoma: produto inexistente responde 200 com "erro" no corpo.
    //  A parte B está no método Criar, logo abaixo.
    //  Sua tarefa: fazer o status code dizer a verdade.
    // ══════════════════════════════════════════════════════════════════════
    [HttpGet("{id:int}")]
    public IActionResult Obter(int id)
    {
        var produto = _db.Produtos.Find(id);
        if (produto is null) return Ok(new { erro = "Produto não encontrado" });
        return Ok(produto);
    }

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 02 · AULA 02 — Parte B (não corrija ainda!)
    //  Sintoma: criar um recurso responde 200, sem dizer onde ele ficou.
    //  Sua tarefa: responder como manda o HTTP para uma criação.
    // ══════════════════════════════════════════════════════════════════════
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Criar(NovoProduto novo)
    {
        var produto = new Produto { Nome = novo.Nome, Preco = novo.Preco, Estoque = novo.Estoque };
        _db.Produtos.Add(produto);
        _db.SaveChanges();
        return Ok(produto);
    }

    // ══════════════════════════════════════════════════════════════════════
    //  FALHA 09 · AULA 09 — RBAC (não corrija ainda!)
    //  Parte A. A parte B (ABAC) está em PedidosController.
    //  Especificação: só o papel Admin remove produtos.
    //  Sintoma: João (Cliente) apaga o catálogo; a API pergunta QUEM ele é,
    //  mas não o que ele PODE fazer.
    //  Sua tarefa: traduzir a especificação para o atributo. Esperado: 403.
    // ══════════════════════════════════════════════════════════════════════
    [Authorize]
    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        var produto = _db.Produtos.Find(id);
        if (produto is null) return NotFound();

        _db.Produtos.Remove(produto);
        _db.SaveChanges();
        return NoContent();
    }
}
