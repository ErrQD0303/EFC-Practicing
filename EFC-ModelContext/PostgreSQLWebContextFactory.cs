using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC_Interfaces;
using EFC_Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EFC_ModelContext
{
    public class PostgreSQLWebContextFactory : IDesignTimeDbContextFactory<PostgreSQLWebContext>
    {
        public PostgreSQLWebContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSQLWebContext>();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? "", "EFC-Main"))
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false);
            var configRoot = configBuilder.Build();
            //optionsBuilder.UseSqlServer(configRoot["database:SQLServer"]);
            optionsBuilder.UseNpgsql(configRoot["database:PostgreSQL"]);
            //optionsBuilder.AddLogging("SQLServer", "WebContext");
            optionsBuilder.AddLogging("PostgreSQL", "WebContext");

            return new PostgreSQLWebContext(optionsBuilder.Options);
        }
    }
}