using LojaDev.Compartilhado.Fila;
using LojaDev.LojaApi.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registra a fila como Singleton — a MESMA instância é compartilhada
// entre o PedidosController (produtor) e o NotificacaoBackground (consumidor).
// Em produção, o broker de mensagens (RabbitMQ, etc.) faria esse papel.
builder.Services.AddSingleton<FilaDePedidos>();

// Registra o HttpClient que será usado para chamar os outros serviços.
// IHttpClientFactory gerencia o ciclo de vida das conexões HTTP.
builder.Services.AddHttpClient();

// Registra o serviço de background que consome a fila.
// BackgroundService roda em paralelo com a API, sem bloquear as requisições.
builder.Services.AddHostedService<NotificacaoBackground>();

var app = builder.Build();
app.MapControllers();

Console.WriteLine("🛒 LojaApi rodando em http://localhost:5090");
Console.WriteLine("   → PagamentoApi esperado em http://localhost:5091");
Console.WriteLine("   → NotificacaoApi esperado em http://localhost:5092");
app.Run();
