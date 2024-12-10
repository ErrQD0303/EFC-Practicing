using System.Reflection.Metadata.Ecma335;
using EFC_DBActions;
using EFC_Interfaces;
using EFC_Models;
using EFC_PostgresqlConnection;
using EFC_SQLServerConnection;

namespace Examples;

public static class ClassExamples
{
    private static readonly IEFDBConnection _efSQLServerConnection = new EFSQLServerConnection();
    private static readonly IEFDBConnection _pgsqlConnection = new EFPostgresqlConnection();

    public static async Task CreateDatabaseOnSqlServer()
    {
        await CreateDatabase(_efSQLServerConnection);
    }
    public static async Task CreateDatabaseOnPostgresql()
    {
        await CreateDatabase(_pgsqlConnection);
    }
    public static async Task CreateDatabase(IEFDBConnection connection)
    {
        var dbActions = new DBProductActions(connection);
        await dbActions.CreateDatabase();
    }

    public static async Task DeleteDatabaseOnSqlServer()
    {
        await DeleteDatabase(_efSQLServerConnection);
    }
    public static async Task DeleteDatabaseOnPostgresql()
    {
        await DeleteDatabase(_pgsqlConnection);
    }
    public static async Task DeleteDatabase(IEFDBConnection connection)
    {
        var dbActions = new DBProductActions(connection);
        await dbActions.DeleteDatabase();
    }

    public static async Task InsertRecordsToProductTableOnSqlServer()
    {
        await InsertRecordsToProductTable(_efSQLServerConnection);
    }
    public static async Task InsertRecordsToProductTableOnPostgresql()
    {
        await InsertRecordsToProductTable(_pgsqlConnection);
    }
    public static async Task InsertRecordsToProductTable(IEFDBConnection connection)
    {
        var dbActions = new DBProductActions(connection);
        var products = new List<Product>
        {
            new() {
                Name = "Product1",
                Provider = "Company 1"
            },
            new() {
                Name = "Product2",
                Provider = "Company 2"
            }
        };
        await dbActions.Insert(products);
    }

    public static async Task ReadRecordsFromProductTableOnSqlServer()
    {
        await ReadRecordsFromProductTable(_efSQLServerConnection);
    }
    public static async Task ReadRecordsFromProductTableOnPostgresql()
    {
        await ReadRecordsFromProductTable(_pgsqlConnection);
    }
    public static async Task ReadRecordsFromProductTable(IEFDBConnection connection)
    {
        var dbActions = new DBProductActions(connection);
        System.Console.WriteLine("Read all records from Product table:");
        await dbActions.SelectAll();
        System.Console.WriteLine("Read record by ID:1");
        await dbActions.SelectById(1);
        System.Console.WriteLine("Read record by Provider:provider2");
        await dbActions.SelectByProvider("provider2");
        System.Console.WriteLine("Read record by Provider:'Company 2'");
        await dbActions.SelectByProvider("Company 2");
    }

    public static async Task UpdateRecordInProductTableOnSqlServer()
    {
        await UpdateRecordInProductTable(_efSQLServerConnection);
    }
    public static async Task UpdateRecordInProductTableOnPostgresql()
    {
        await UpdateRecordInProductTable(_pgsqlConnection);
    }
    public static async Task UpdateRecordInProductTable(IEFDBConnection connection)
    {
        var dbActions = new DBProductActions(connection);
        System.Console.WriteLine("Update record by ID:1");
        var product = new Product
        {
            Name = "ProductA1",
            Provider = "Company 1"
        };
        await dbActions.Update(1, product);
        System.Console.WriteLine("Update record's name by ID:2");
        var newName = "ProductA2";
        await dbActions.UpdateName(2, newName);
        System.Console.WriteLine("Update record's provider by ID:2");
        var newProvider = "Company A2";
        await dbActions.UpdateProvider(2, newProvider);
    }

    public static async Task DeleteRecordFromProductTableOnSqlServer()
    {
        await DeleteRecordFromProductTable(_efSQLServerConnection);
    }
    public static async Task DeleteRecordFromProductTableOnPostgresql()
    {
        await DeleteRecordFromProductTable(_pgsqlConnection);
    }
    public static async Task DeleteRecordFromProductTable(IEFDBConnection connection)
    {
        var dbActions = new DBProductActions(connection);
        System.Console.WriteLine("Delete record by ID:1");
        await dbActions.Delete(1);
        System.Console.WriteLine("Current records in Product table:");
        await dbActions.SelectAll();
    }
}
