using System.Text;
using LojaRevisao.Api.Data;
using LojaRevisao.Api.Modulos.Catalogo;
using LojaRevisao.Api.Modulos.Notificacoes;
using LojaRevisao.Api.Modulos.Pedidos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ══════════════════════════════════════════════════════════════════════════
//  FALHA 10 · AULA 10 — Contrato OpenAPI (não corrija ainda!)
//  Parte B. A parte A está em PedidosController.
//  Sintoma: a API exige JWT, mas o contrato não fala em segurança: o Swagger
//  UI não tem o botão Authorize e ninguém consegue testar as rotas protegidas.
//  Sua tarefa: declarar o esquema Bearer (JWT) no contrato.
// ══════════════════════════════════════════════════════════════════════════
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Loja")));

builder.Services.AddScoped<ICatalogo, Catalogo>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddSingleton<EmailGatewaySimulado>();
builder.Services.AddSingleton<FilaDeNotificacoes>();
builder.Services.AddHostedService<NotificacaoWorker>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero,

            // ══════════════════════════════════════════════════════════════
            //  FALHA 08 · AULA 08 — Autenticação com JWT (não corrija ainda!)
            //  Sintoma: um token vencido há 5 minutos continua abrindo a API.
            //  Um token roubado valeria para sempre.
            //  Teste: pegue um token em POST /api/v1/auth/login-expirado e
            //  chame GET /api/v1/pedidos. Sua tarefa: fazer essa chamada dar 401.
            // ══════════════════════════════════════════════════════════════
            ValidateLifetime = false,
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Banco zerado a cada execução: reiniciar a API restaura os dados da prática.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LojaDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    DadosIniciais.Popular(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
