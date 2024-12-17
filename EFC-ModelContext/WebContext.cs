using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC_Attributes;
using EFC_Interfaces;
using EFC_Logger;
using EFC_Models.WebModels;
using Helpers;
using Microsoft.EntityFrameworkCore;

namespace EFC_ModelContext
{
    public class WebContext<T> : DbContext where T : WebContext<T>
    {
        IEFDBConnection? _efConnection;

        public WebContext(DbContextOptions<T> options) : base(options) { }

        public WebContext(IEFDBConnection efConnection)
        {
            _efConnection = efConnection;
        }

        public DbSet<Article>? Articles { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<ArticleTag>? ArticleTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            _efConnection?.UseDatabase(optionsBuilder)
                .AddLogging(_efConnection.GetType().Name, this.GetType().ToString()); // Login Configs
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

            // Many To Many Mapping
            modelBuilder.Entity<ArticleTag>(entity =>
            {
                entity.HasIndex(articleTag => new
                {
                    articleTag.ArticleId,
                    articleTag.TagId
                })
                    .IsUnique();
            });
        }
    }
}