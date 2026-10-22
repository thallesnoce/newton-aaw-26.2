using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BibliotecaApi.Controllers;

// PRÉ-PRONTO — NÃO é o foco desta aula (JWT foi tema da Aula 07).
// Existe para que a API tenha endpoints protegidos e o botão "Authorize"
// do Swagger tenha o que autorizar. Credenciais didáticas fixas abaixo.
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    public record CredenciaisRequest(string Usuario, string Senha);
    public record TokenResponse(string Token, DateTime ExpiraEm);

    [HttpPost("login")]
    public IActionResult Login([FromBody] CredenciaisRequest credenciais)
    {
        // Usuário didático fixo: bibliotecario / newton123
        if (credenciais.Usuario != "bibliotecario" || credenciais.Senha != "newton123")
            return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });

        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Chave"]!));
        var expiraEm = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Emissor"],
            audience: _config["Jwt:Audiencia"],
            claims: new[]
            {
                new Claim(ClaimTypes.Name, credenciais.Usuario),
                new Claim(ClaimTypes.Role, "Bibliotecario"),
            },
            expires: expiraEm,
            signingCredentials: new SigningCredentials(chave, SecurityAlgorithms.HmacSha256));

        return Ok(new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiraEm));
    }
}
