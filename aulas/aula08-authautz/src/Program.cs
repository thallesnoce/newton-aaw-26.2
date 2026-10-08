using BibliotecaApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Acervo em memória (compartilhado por toda a aplicação).
builder.Services.AddSingleton<BibliotecaStore>();

// ╔══════════════════════════════════════════════════════════════════╗
// ║  ETAPA 1 — Configurar a autenticação JWT                         ║
// ╚══════════════════════════════════════════════════════════════════╝
// Os pacotes já estão no .csproj (veja o comentário lá). Em um projeto
// novo você rodaria:
//   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
//
// TODO 1.1: adicione os usings no topo deste arquivo:
//   using System.Text;
//   using Microsoft.AspNetCore.Authentication.JwtBearer;
//   using Microsoft.IdentityModel.Tokens;
//
// TODO 1.2: registre a autenticação, dizendo à API como VALIDAR o token:
//
//   var jwt = builder.Configuration.GetSection("Jwt");
//   builder.Services.AddAuthentication(options =>
//   {
//       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//   })
//   .AddJwtBearer(options =>
//   {
//       options.TokenValidationParameters = new TokenValidationParameters
//       {
//           ValidateIssuer = true,
//           ValidateAudience = true,
//           ValidateLifetime = true,          // token expirado => 401
//           ValidateIssuerSigningKey = true,  // assinatura inválida => 401
//           ValidIssuer = jwt["Issuer"],
//           ValidAudience = jwt["Audience"],
//           IssuerSigningKey = new SymmetricSecurityKey(
//               Encoding.UTF8.GetBytes(jwt["Key"]!)),
//           ClockSkew = TimeSpan.Zero         // sem os 5 min de tolerância padrão
//       };
//   });
//
//   builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO 1.3: ligue os middlewares, NESTA ORDEM:
//   app.UseAuthentication();   // quem é você?  (lê e valida o token)
//   app.UseAuthorization();    // o que pode fazer? (checa [Authorize])
//
// Inverter a ordem faz todo [Authorize] falhar — teste depois, vale a pena ver.

app.MapControllers();

app.Run();
