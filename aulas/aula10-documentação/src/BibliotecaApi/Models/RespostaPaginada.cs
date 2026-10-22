namespace BibliotecaApi.Models;

public class RespostaPaginada<T>
{
    public List<T> Itens { get; set; } = new();

    public int Pagina { get; set; }

    public int TamanhoPagina { get; set; }

    public int TotalItens { get; set; }

    public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / TamanhoPagina);
}
