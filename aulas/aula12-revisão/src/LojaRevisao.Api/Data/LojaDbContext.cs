using LojaRevisao.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaRevisao.Api.Data;

public class LojaDbContext : DbContext
{
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options) { }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
}
