using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaApi.Autorizacao;

// ═══════════════════════════════════════════════════════════════════
//  ABAC — Attribute-Based Access Control
//  A decisão olha ATRIBUTOS, não só o papel:
//    • do SUJEITO  → id do usuário (claim NameIdentifier) e papel
//    • do RECURSO  → quem é o dono do empréstimo (UsuarioId)
//    • da AÇÃO     → renovar
//  É a correção formal do IDOR que vocês exploraram na Aula 07.
// ═══════════════════════════════════════════════════════════════════

/// <summary>Requirement: "só renova o empréstimo quem tem direito sobre ELE".</summary>
public class DonoDoEmprestimoRequirement : IAuthorizationRequirement { }

/// <summary>
/// Handler: a decisão. Regra de negócio esperada (matriz no README):
///   - Bibliotecário renova QUALQUER empréstimo;
///   - Leitor e Pesquisador renovam SOMENTE os PRÓPRIOS empréstimos.
/// </summary>
public class DonoDoEmprestimoHandler : AuthorizationHandler<DonoDoEmprestimoRequirement, Emprestimo>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DonoDoEmprestimoRequirement requirement,
        Emprestimo emprestimo)
    {
        // ┌──────────────────────────────────────────────────────────┐
        // │ FALHA 3 (não corrija ainda!)                              │
        // │ Este handler se diz ABAC, mas só olha o PAPEL do usuário. │
        // │ O atributo do recurso (emprestimo.UsuarioId) nem é lido.  │
        // └──────────────────────────────────────────────────────────┘
        if (context.User.IsInRole("Leitor")
            || context.User.IsInRole("Pesquisador")
            || context.User.IsInRole("Bibliotecario"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
