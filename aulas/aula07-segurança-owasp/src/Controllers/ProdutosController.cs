using ApiVazada.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiVazada.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProdutosController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Produtos.ToList());

    // ══════════════════════════════════════════════════════════════════
    //  FALHA 1 (não corrija ainda!) — Injeção de SQL
    //  O termo digitado pelo usuário é concatenado direto na consulta.
    //  Quem controla a entrada controla o SQL que o banco executa.
    //  Teste: GET /api/produtos/buscar?nome=Note
    //         GET /api/produtos/buscar?nome=%' OR '1'='1
    // ══════════════════════════════════════════════════════════════════
    [HttpGet("buscar")]
    public IActionResult Buscar(string nome)
    {
        var sql = $"SELECT * FROM Produtos WHERE Nome LIKE '%{nome}%'";
        var resultado = _db.Produtos.FromSqlRaw(sql).ToList();
        return Ok(resultado);
    }

    /// <summary>
    /// Endpoint que sempre falha — use para observar a FALHA 3 (Parte B):
    /// veja quanta informação do servidor a resposta de erro entrega.
    /// </summary>
    [HttpGet("quebrar")]
    public IActionResult Quebrar()
    {
        throw new InvalidOperationException(
            "Erro proposital para demonstrar a exposição de stack trace.");
    }
}
