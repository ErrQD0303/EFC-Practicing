using EFC_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFC_PostgresqlConnection;

public class EFPostgresqlConnection : IEFDBConnection
{
    public EFPostgresqlConnection()
    {
    }

    string IEFDBConnection.ConnectionString => "PostgresSQL";

    public void UseDatabase(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = ((IEFDBConnection)this).GetConnectionString();
        optionsBuilder.UseNpgsql(connectionString);
    }
}
