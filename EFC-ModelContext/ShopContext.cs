using DTOs;
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
    public DbSet<CategoryDetail>? CategoryDetails { get; set; }
    public DbSet<User>? Users { get; set; }

    // Keyless Dbset
    public DbSet<CategoryProductDto>? CategoryProductDtos { get; set; }

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

        //Docs: https://learn.microsoft.com/en-us/ef/core/modeling/
        // Example of Fluent API with Product Model without using Attribute Notation
        modelBuilder.Entity<Product>(entity =>
        {
            // Entity => Fluent API
            entity.ToTable("SanPham"); // You can remove the Table Attribute in the Model now
            // PK
            entity.HasKey(p => p.Id);

            // Index
            entity.HasIndex(p => p.Price)
                .HasDatabaseName("index-sanpham-price");

            // Relative
            entity.HasOne(p => p.Category)
                .WithMany() // Map One-to-Many with Category
                .HasForeignKey("CategoryId") // Name foreign key
                .OnDelete(DeleteBehavior.Cascade) // On Delete Cascade
                .HasConstraintName("Khoa_ngoai_SanPham_Category") // Constraint
                ;

            entity.HasOne(p => p.Category2)
                .WithMany(c => c.Products) // Map Many-to-Many with Category using Inverse Property
                .HasForeignKey("CategoryId2") // Name foreign key
                .OnDelete(DeleteBehavior.NoAction) // On Delete No Action
                .HasConstraintName("Khoa_ngoai_SanPham_Category2") // Constraint
                ;

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("title")
                .HasColumnType(AppConstants
                    .MODEL_TYPE_MAPPINGS[nameof(Product)][nameof(Product.Name)]
                    .FirstOrDefault(val => val.StartsWith(provider))
                    ?.Split(":")[1])
                .HasDefaultValue("Ten san pham mac dinh")
                ;

            entity.Property(p => p.Provider)
                .HasMaxLength(100)
                ;

            entity.Property(p => p.Price)
                .HasColumnType(AppConstants
                    .MODEL_TYPE_MAPPINGS[nameof(Product)][nameof(Product.Price)]
                    .FirstOrDefault(val => val.StartsWith(provider))
                    ?.Split(":")[1])
                ;

            entity.HasOne(p => p.UserPost)
                .WithMany(u => u.ProductsPost) // Map One-to-Many with User using ForeignKey
                .HasForeignKey("UserPostId")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Products_user_1234");
        });

        // 1 - 1 Mapping (For extending Category Model)
        modelBuilder.Entity<CategoryDetail>(entity =>
        {
            entity.ToTable("CategoryDetail");

            entity.HasOne(cd => cd.Category)
                .WithOne(c => c.CategoryDetail)
                .HasForeignKey<CategoryDetail>(cd => cd.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
            ;
        });

        // Config Keyless Entity
        modelBuilder.Entity<CategoryProductDto>().HasNoKey().ToView(null);
    }
}
