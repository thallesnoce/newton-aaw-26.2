using System.Security.Claims;
using BibliotecaApi.Models;
using Microsoft.Extensions.Options;

namespace BibliotecaApi.Autorizacao;

// ═══════════════════════════════════════════════════════════════════
//  PBAC — Policy-Based Access Control
//  As regras de acesso NÃO estão no código: estão em politicas.json,
//  um documento de política versionável e auditável, mantido por quem
//  responde pela regra de negócio. Este motor só AVALIA o documento.
//
//    PAP (onde a política é escrita) .... politicas.json
//    PDP (quem decide) .................. MotorDePoliticas  ← este arquivo
//    PEP (quem aplica a decisão) ........ EmprestimosController
//
//  Algoritmo de combinação: "PRIMEIRA APLICÁVEL" (first-applicable,
//  o mesmo nome usado no XACML):
//    1. percorre as regras NA ORDEM do arquivo;
//    2. a primeira cujo ALVO (Acao + Categoria) casa com o pedido decide:
//       papel do usuário está em Papeis → PERMITE; senão → NEGA;
//    3. nenhuma regra casou → NEGA (negar por padrão).
//
//  O arquivo é recarregado a cada alteração: mudar a política NÃO exige
//  recompilar nem reiniciar a API.
//
//  Este motor está CORRETO. Se algo for permitido indevidamente,
//  o problema está na POLÍTICA — vá ao politicas.json.
// ═══════════════════════════════════════════════════════════════════

public class RegraDePolitica
{
    public string Nome { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty;
    /// <summary>Categoria do livro ou "*" para qualquer categoria.</summary>
    public string Categoria { get; set; } = "*";
    public List<string> Papeis { get; set; } = new();
}

public class PoliticasOptions
{
    public List<RegraDePolitica> Regras { get; set; } = new();
}

/// <summary>Resultado da avaliação — inclui QUAL regra decidiu (rastreabilidade).</summary>
public record Decisao(bool Permitido, string Regra);

public class MotorDePoliticas
{
    private readonly IOptionsMonitor<PoliticasOptions> _politicas;

    public MotorDePoliticas(IOptionsMonitor<PoliticasOptions> politicas) => _politicas = politicas;

    public Decisao Avaliar(ClaimsPrincipal usuario, string acao, Livro livro)
    {
        foreach (var regra in _politicas.CurrentValue.Regras)
        {
            var alvoCasa = regra.Acao == acao
                && (regra.Categoria == "*" || regra.Categoria == livro.Categoria);

            if (!alvoCasa) continue;

            var permitido = regra.Papeis.Any(usuario.IsInRole);
            return new Decisao(permitido, regra.Nome);
        }

        return new Decisao(false, "(nenhuma regra aplicável — negado por padrão)");
    }
}
