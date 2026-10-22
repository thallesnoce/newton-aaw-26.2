using System.Text;
using BibliotecaApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Repositório em memória (Singleton para manter dados entre requisições)
builder.Services.AddSingleton<BibliotecaRepository>();

builder.Services.AddControllers();

// ---------------------------------------------------------------
// Autenticação JWT — PRÉ-PRONTA (tema da Aula 07, não desta aula).
// Está aqui para que existam endpoints protegidos e o Swagger tenha
// o que autorizar no Passo 9 (security scheme + botão Authorize).
// ---------------------------------------------------------------
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emissor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Chave"]!)),
        };
    });
builder.Services.AddAuthorization();

// TODO: Configurar Swagger/OpenAPI aqui (Passo 2)
//   Nos passos seguintes, o AddSwaggerGen ganhará: metadados (Passo 5),
//   XML comments (Passo 6) e o security scheme Bearer (Passo 9).

var app = builder.Build();

// TODO: Adicionar middlewares do Swagger aqui (Passo 3)

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
