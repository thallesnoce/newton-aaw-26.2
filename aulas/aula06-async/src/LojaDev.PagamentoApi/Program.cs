var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

Console.WriteLine("💳 PagamentoApi rodando em http://localhost:5091");
app.Run();
