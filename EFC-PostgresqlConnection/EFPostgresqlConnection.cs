using EFC_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFC_PostgresqlConnection;

public class EFPostgresqlConnection : IEFDBConnection
{
    public EFPostgresqlConnection()
    {
    }

    string IEFDBConnection.ConnectionString => "PostgresSQL";

    public DbContextOptionsBuilder UseDatabase(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = ((IEFDBConnection)this).GetConnectionString();
        return optionsBuilder.UseNpgsql(connectionString);
    }
}
