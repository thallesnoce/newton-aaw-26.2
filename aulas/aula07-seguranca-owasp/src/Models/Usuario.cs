namespace ApiVazada.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Define o que o usuário pode fazer no sistema: "User" ou "Admin".
    /// Só deveria mudar por uma operação administrativa — nunca por um
    /// campo enviado pelo próprio cliente.
    /// </summary>
    public string Role { get; set; } = "User";
}
