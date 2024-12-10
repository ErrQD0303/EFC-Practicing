using EFC_Interfaces;
using EFC_ModelContext;
using EFC_Models;
using Helpers;
using Microsoft.EntityFrameworkCore;

namespace EFC_DBActions;

public class DBProductActions : IDBActions<Product>
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

    public async Task<bool> Insert(params IEnumerable<Product> products)
    {
        using var dbContext = new ProductsContext(_efConnection);
        await dbContext.AddRangeAsync(products);

        // Update DbContext's new records into Server
        int addedRow = await dbContext.SaveChangesAsync();
        System.Console.WriteLine($"{addedRow} row(s) added");
        return true;
    }

    public async Task<IEnumerable<Product>?> SelectAll()
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table is empty");
            return null;
        }
        var products = await dbContext.products.ToListAsync();
        products.ShowProducts();
        return products;
    }

    public async Task<Product?> SelectById(int id)
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table does not exist");
            return null;
        }
        var product = dbContext.products
            .Find(id);
        new List<Product?> { product }.ShowProducts();
        return product;
    }

    public async Task<IEnumerable<Product>?> SelectByName(string name)
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table is empty");
            return null;
        }
        var products = await dbContext.products
            .Where(p => p.Name == name)
            .ToListAsync();
        products.ShowProducts();
        return products;
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

    public async Task<Product?> Update(int id, Product product)
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table does not exist");
            return null;
        }

        var productFromDB = await dbContext.products.FindAsync(id);
        if (productFromDB is null)
        {
            System.Console.WriteLine("Product not found");
            return null;
        }

        foreach (var property in dbContext.Entry(productFromDB).Properties.Where(p => p.Metadata.Name != "ProductId"))
        {
            var newValue = dbContext.Entry(product).Property(property.Metadata.Name).CurrentValue;
            var oldValue = property.CurrentValue;
            if (newValue is null || newValue.ToString() == "" || Equals(newValue, oldValue))
            {
                continue;
            }
            property.CurrentValue = newValue;
            property.IsModified = true;
        }

        // dbContext.products.Update(product);
        await dbContext.SaveChangesAsync();
        System.Console.WriteLine("Product updated");

        var returnProduct = await dbContext.products.FindAsync(id);
        new List<Product?>() { returnProduct }.ShowProducts();

        return returnProduct;
    }
    public async Task<Product?> UpdateName(int id, string name)
    {
        return await Update(id, new Product { Name = name });
    }
    public async Task<Product?> UpdateProvider(int id, string provider)
    {
        return await Update(id, new Product { Provider = provider });
    }

    public async Task<bool> Delete(int id)
    {
        using var dbContext = new ProductsContext(_efConnection);
        if (dbContext.products is null)
        {
            System.Console.WriteLine("Table does not exist");
            return false;
        }
        var product = await dbContext.products.FindAsync(id);
        if (product is null)
        {
            System.Console.WriteLine("Product not found");
            return false;
        }
        dbContext.products.Remove(product);
        return await dbContext.SaveChangesAsync() > 0;
    }
}
