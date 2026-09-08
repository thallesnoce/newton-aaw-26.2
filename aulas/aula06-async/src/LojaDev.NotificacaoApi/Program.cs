var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

Console.WriteLine("📧 NotificacaoApi rodando em http://localhost:5092");
app.Run();
