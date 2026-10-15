using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BibliotecaApi.Controllers;

/// <summary>
/// Login pronto — é a solução da Aula 08. Nada a corrigir aqui.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    /// <summary>POST /api/auth/login — valida as credenciais e devolve um JWT.</summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var usuario = BibliotecaStore.Autenticar(request.Email, request.Senha);
        if (usuario is null)
            return Unauthorized(new { message = "Credenciais inválidas." });

        return Ok(new { token = GerarToken(usuario), nome = usuario.Nome, role = usuario.Role });
    }

    /// <summary>GET /api/quem-sou-eu — mostra as claims (atributos do SUJEITO) do token.</summary>
    [AllowAnonymous]
    [HttpGet("/api/quem-sou-eu")]
    public IActionResult QuemSouEu()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Ok(new { autenticado = false, dica = "Faça login e mande o header Authorization: Bearer <token>." });

        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new { autenticado = true, claims });
    }

    private string GerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),  // atributo usado pelo ABAC
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Role, usuario.Role),                     // atributo usado pelo RBAC e pelo PBAC
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresInMinutes"] ?? "60")),
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
