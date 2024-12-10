using EFC_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFC_SQLServerConnection;

public class EFSQLServerConnection : IEFDBConnection
{
    public EFSQLServerConnection()
    {
    }
    string IEFDBConnection.ConnectionString => "SQLServer";

    public void UseDatabase(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = ((IEFDBConnection)this).GetConnectionString();
        optionsBuilder.UseSqlServer(connectionString);
    }
}
