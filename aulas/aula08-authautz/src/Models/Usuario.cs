namespace BibliotecaApi.Models;

/// <summary>
/// Usuário fictício da biblioteca. Em um sistema real, viria de um banco
/// com a senha guardada como hash — aqui é texto puro só para a aula.
/// </summary>
public record Usuario(int Id, string Nome, string Email, string Senha, string Role);

/// <summary>Corpo do POST /api/auth/login.</summary>
public record LoginRequest(string Email, string Senha);
