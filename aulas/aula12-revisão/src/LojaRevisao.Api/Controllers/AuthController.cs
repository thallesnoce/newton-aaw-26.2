using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LojaRevisao.Api.Data;
using LojaRevisao.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LojaRevisao.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    /// <summary>Valida as credenciais e devolve um JWT.</summary>
    [HttpPost("login")]
    public IActionResult Login(Credenciais credenciais)
    {
        var usuario = Autenticar(credenciais);
        if (usuario is null) return Unauthorized();

        var minutos = int.Parse(_config["Jwt:ExpiresInMinutes"]!);
        var token = GerarToken(usuario, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(minutos));
        return Ok(new { token, usuarioId = usuario.Id, role = usuario.Role });
    }

    /// <summary>Só para a prática: emite um token que já nasce vencido, para testar o 401.</summary>
    [HttpPost("login-expirado")]
    public IActionResult LoginExpirado(Credenciais credenciais)
    {
        var usuario = Autenticar(credenciais);
        if (usuario is null) return Unauthorized();

        var token = GerarToken(usuario, DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow.AddMinutes(-5));
        return Ok(new { token, aviso = "Este token já está expirado." });
    }

    private static UsuarioFicticio? Autenticar(Credenciais c) =>
        DadosIniciais.Usuarios.FirstOrDefault(u => u.Email == c.Email && u.Senha == c.Senha);

    private string GerarToken(UsuarioFicticio usuario, DateTime inicio, DateTime fim)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role),
        };

        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            notBefore: inicio,
            expires: fim,
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
