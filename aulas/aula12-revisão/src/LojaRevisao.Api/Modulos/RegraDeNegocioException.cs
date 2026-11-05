namespace LojaRevisao.Api.Modulos;

/// <summary>Erro de regra de negócio; o controller o converte em Problem Details.</summary>
public class RegraDeNegocioException : Exception
{
    public int Status { get; }
    public string Titulo { get; }

    private RegraDeNegocioException(int status, string titulo, string detalhe) : base(detalhe)
    {
        Status = status;
        Titulo = titulo;
    }

    public static RegraDeNegocioException NaoEncontrado(string detalhe) =>
        new(StatusCodes.Status404NotFound, "Recurso não encontrado", detalhe);

    public static RegraDeNegocioException Conflito(string detalhe) =>
        new(StatusCodes.Status409Conflict, "Conflito com o estado atual", detalhe);
}
