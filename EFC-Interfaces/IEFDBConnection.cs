using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFC_Interfaces;

public interface IEFDBConnection
{
    virtual string GetConnectionString()
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new ArgumentNullException("ConnectionString");
        }

        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false);
        var configRoot = configBuilder.Build();
        return configRoot[$"database:{ConnectionString}"] ?? throw new Exception("Connection string not found");
    }

    protected string ConnectionString { get; }
    void UseDatabase(DbContextOptionsBuilder optionsBuilder);
}
