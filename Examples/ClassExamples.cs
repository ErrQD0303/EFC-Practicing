using EFC_DBActions;
using EFC_Models;
using EFC_PostgresqlConnection;
using EFC_Repositories;
using EFC_SQLServerConnection;
using Helpers;

namespace Examples;

public static class ClassExamples
{
    private static readonly UnitOfWork _unitOfWork = new UnitOfWork(new EFSQLServerConnection(), new EFPostgresqlConnection());

    public static async Task CreateDatabase()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        await dbActions.CreateDatabase();
    }

    public static async Task DeleteDatabase()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        await dbActions.DeleteDatabase();
    }

    public static async Task InsertRecords()
    {
        var dbActions = new DBShopActions(_unitOfWork);

        var cate1 = new Category() { Name = "Cate1", Description = "Description1" };
        var cate2 = new Category() { Name = "Cate2", Description = "Description2" };
        await dbActions.AddAsync(cate1, cate2);
        var products = new List<Product>
        {
            new()  {Name = "Sản phẩm 1",    Price=12, Category = cate2, Provider = "provider1"},
            new()  {Name = "Sản phẩm 2",    Price=11, Category = cate2, Provider = "provider1"},
            new()  {Name = "Sản phẩm 3",    Price=33, Category = cate2, Provider = "provider2"},
            new()  {Name = "Sản phẩm 4(1)", Price=323, Category = cate1, Provider = "provider2"},
            new()  {Name = "Sản phẩm 5(1)", Price=333, Category = cate1, Category2 = cate2, Provider = "provider2"},
        };
        await dbActions.AddAsync(products);
    }

    public static async Task ReadRecordsFromProductTable()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine("Read all records from Product table:");
        await dbActions.GetAllAsync<Product>();
        System.Console.WriteLine("Read record by ID:1");
        var product1 = await dbActions.GetProductByIdAsync(1);
        System.Console.WriteLine("Read record by Provider:provider2");
        await dbActions.GetProductByProvider("provider2");
        System.Console.WriteLine("Read record by Provider:'Company 2'");
        await dbActions.GetProductByProvider("Company 2");
        System.Console.WriteLine($"Products with price greater than 30$:");
        var products2 = await dbActions.GetByConditionAsync<Product>(p => p.Price > 30);
        products2?.ShowItems();
    }

    public static async Task ReadRecordsFromCategoryTable()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine("Read all records from Category table:");
        var categories = await dbActions.GetAllAsync<Category>();
        System.Console.WriteLine("Read record by ID:1");
        var category = await dbActions.GetCategoryByIdAsync(1);
        System.Console.WriteLine($"All Products of Category {category?.Id}:");
        category?.Products.ShowItems();
    }

    public static async Task UpdateRecordInProductTable()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine("Update record by ID:1");
        var product = new Product
        {
            Name = "ProductA1",
            Provider = "Company 1",
            CategoryId = 2
        };
        await dbActions.UpdateAsync(1, product);
        System.Console.WriteLine("Update record's name by ID:2");
        var newName = "ProductA2";
        await dbActions.UpdateName(2, newName);
        System.Console.WriteLine("Update record's provider by ID:2");
        var newProvider = "Company A2";
        await dbActions.UpdateProvider(2, newProvider);
    }

    public static async Task DeleteRecordFromProductTable()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine("Delete record by ID:1");
        await dbActions.DeleteAsync<Product>(1);
        System.Console.WriteLine("Current records in Product table:");
        await dbActions.GetAllAsync<Product>();
    }

    public static async Task DeleteRecordFromCategoryTable()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine("Delete record by ID:1");
        await dbActions.DeleteAsync<Category>(1);
    }
}
