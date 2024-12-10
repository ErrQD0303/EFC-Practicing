using EFC_Interfaces;
using EFC_Models;
using Microsoft.EntityFrameworkCore;

namespace EFC_ModelContext;

public class ProductsContext : DbContext
{
    IEFDBConnection _efConnection;

    public ProductsContext(IEFDBConnection efConnection)
    {
        _efConnection = efConnection;
    }

    public DbSet<Product>? products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        _efConnection.UseDatabase(optionsBuilder);
    }
}
