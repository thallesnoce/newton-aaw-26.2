using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiVazada.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiVazada.Controllers;

/// <summary>
/// Login de apoio: emite o token usado para testar as demais falhas.
/// Não é alvo da prática — não há falha marcada aqui.
/// </summary>
[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _db;

    public LoginController(AppDbContext db) => _db = db;

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _db.Usuarios.FirstOrDefault(u =>
            u.Email == request.Email && u.Senha == request.Senha);

        if (user is null) return Unauthorized(new { message = "Credenciais inválidas." });

        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("S3cr3t_K3y_That_Is_L0ng_En0ugh_F0r_HMAC_SHA256");

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return Ok(new { token = handler.WriteToken(token), usuarioId = user.Id, role = user.Role });
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
