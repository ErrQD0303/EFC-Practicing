using EFC_Interfaces;
using EFC_ModelContext;
using EFC_Models;
using Helpers;
using Microsoft.EntityFrameworkCore;

namespace EFC_DBActions;

public class DBProductActions : IDBActions
{
    private readonly IEFDBConnection _efConnection;

    public DBProductActions(IEFDBConnection efConnection)
    {
        _efConnection = efConnection ?? throw new ArgumentNullException(nameof(efConnection));
    }

    public async Task CreateDatabase()
    {
        using var dbContext = new ProductsContext(_efConnection);
        var databaseName = dbContext.Database.GetDbConnection().Database;
        System.Console.WriteLine("Create " + databaseName);
        bool result = await dbContext.Database.EnsureCreatedAsync();
        string resultString = result ? "created" : "already exists";
        System.Console.WriteLine($"Database {databaseName} {resultString}");
    }

    public async Task DeleteDatabase()
    {
        using var dbContext = new ProductsContext(_efConnection);
        var databaseName = dbContext.Database.GetDbConnection().Database;
        System.Console.WriteLine($"Do you want to delete {databaseName}? (y)");
        string? input = Console.ReadLine();
        if (String.Equals(input, "y", StringComparison.OrdinalIgnoreCase))
        {
            var deleted = await dbContext.Database.EnsureDeletedAsync();
            var deletionInfo = deleted ? "deleted" : "not deleted";
            System.Console.WriteLine($"{databaseName} {deletionInfo}");
        }
    }

    public async Task Insert(params IEnumerable<Product> products)
    {
        using var dbContext = new ProductsContext(_efConnection);
        await dbContext.AddRangeAsync(products);

        // Update DbContext's new records into Server
        int addedRow = await dbContext.SaveChangesAsync();
        System.Console.WriteLine($"{addedRow} row(s) added");
    }

    public async Task SelectAll()
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table is empty");
            return;
        }
        var products = await dbContext.products.ToListAsync();
        products.ShowProducts();
    }

    public async Task SelectById(int id)
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table is empty");
            return;
        }
        var product = dbContext.products
            .Find(id);
        new List<Product?> { product }.ShowProducts();
    }

    public async Task SelectByName(string name)
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table is empty");
            return;
        }
        var products = await dbContext.products
            .Where(p => p.Name == name)
            .ToListAsync();
        products.ShowProducts();
    }

    public async Task SelectByProvider(string provider)
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table is empty");
            return;
        }
        var products = await dbContext.products
            .Where(p => p.Provider == provider)
            .ToListAsync();
        products.ShowProducts();
    }
}
