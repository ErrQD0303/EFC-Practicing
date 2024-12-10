using EFC_Interfaces;
using EFC_Logger;
using EFC_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

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

        // Loging configs
        optionsBuilder.AddLogging();
    }
}
