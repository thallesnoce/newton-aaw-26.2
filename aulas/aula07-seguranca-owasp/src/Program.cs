using System.Text;
using ApiVazada.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ══════════════════════════════════════════════════════════════════════
//  FALHA 3 (não corrija ainda!) — Segredos no código e erro exposto
//  Parte A: a connection string e a chave de assinatura do JWT estão
//  escritas aqui, em texto puro, dentro de um arquivo versionado no Git.
// ══════════════════════════════════════════════════════════════════════
const string CONNECTION_STRING = "Data Source=apivazada.db";
const string JWT_SECRET = "S3cr3t_K3y_That_Is_L0ng_En0ugh_F0r_HMAC_SHA256";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(CONNECTION_STRING));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWT_SECRET)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// ══════════════════════════════════════════════════════════════════════
//  FALHA 3 (não corrija ainda!) — Parte B
//  A página de exceção detalhada está ligada SEMPRE, inclusive fora do
//  ambiente de desenvolvimento. Qualquer erro devolve stack trace,
//  caminho de arquivos do servidor, versões de pacotes e trechos do
//  código-fonte ao cliente. Experimente: GET /api/produtos/quebrar
// ══════════════════════════════════════════════════════════════════════
app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
