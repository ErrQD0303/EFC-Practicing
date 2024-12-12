using EFC_Attributes;
using EFC_Interfaces;
using EFC_Logger;
using EFC_Models;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFC_ModelContext;

public class ShopContext : DbContext
{
    IEFDBConnection _efConnection;

    public ShopContext(IEFDBConnection efConnection)
    {
        _efConnection = efConnection;
    }

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        _efConnection.UseDatabase(optionsBuilder)
            .AddLogging(_efConnection.GetType().Name); // Login Configs
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var provider = this.Database.ProviderName;
        if (string.IsNullOrEmpty(provider))
        {
            throw new ArgumentNullException("Provider Name not found!");
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                var dbTypeAttributes = property.GetDBTypesAttributes<DBTypesAttribute>();
                if (dbTypeAttributes is not null && dbTypeAttributes.TypeMappings.ContainsKey(provider))
                {
                    var columnType = dbTypeAttributes.TypeMappings[provider];
                    modelBuilder.Entity(entityType.ClrType)
                                .Property(property.Name)
                                .HasColumnType(columnType);
                }
            }
        }
    }
}
