using DTOs;
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
        var cate3 = new Category() { Name = "Cate3", Description = "Description3" };
        await dbActions.AddAsync(cate1, cate2, cate3);
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

    public static async Task QueryPraticing()
    {
        await Find2ProductWithPriceGreaterThan30WhichHaveTheGreatestPrice();
        await Find2ProductWithPriceGreaterThan30WhichHaveTheLowestPrice();
        await FindProductById6();
        await FindTheNameOfTheFirstProductWithPriceGreaterThan100AndHasNameStartsWithLetterS();
        await PrintTheNameAndPriceOfAllProducts();
        await JoinProductAndCategory();
        await LeftJoinProductAndCategory();
        await RightJoinProductAndCategory();
        await FullOuterJoinProductAndCategory();
    }

    public static async Task RawQueryPraticing()
    {
        await SelectFromProductTable();
        await JoinProductAndCategoryTable();
    }

    public static async Task SelectFromProductTable()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham"; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"All records in Product table:");
        var products = await dbActions.FromSQLRawAsync<Product>("SELECT * FROM \"SanPham\"");
        products?.ShowItems();
    }

    public static async Task JoinProductAndCategoryTable()
    {
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Join Records in Product And Category table:");
        var records = await dbActions.FromSQLRawAsync<CategoryProductDto>("""
            SELECT a."CategoryId", a."CategoryName", a."ProductName", a."ProductId", a."CategoryId2", c2."Name" as "CategoryName2"
            FROM
                (
                    SELECT 
                        s."CategoryId", 
                        c."Name" as "CategoryName",
                        s."Id" as "ProductId",
                        s."title" as "ProductName",
                        s."CategoryId2"
                    FROM 
                        "SanPham" s
                    LEFT JOIN 
                        "Category" c 
                    ON 
                        c."Id" = s."CategoryId"
                ) as a
            LEFT JOIN 
                "Category" c2 
            ON 
                c2."Id" = a."CategoryId2";
        """);
        records?.ShowItems();
    }


    public static async Task LeftJoinProductAndCategory()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham" s 
        LEFT JOIN "Category" c ON s."CategoryId" = c."Id"
        LEFT JOIN "Category" c2 ON s."CategoryId2" = c2."Id"; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Left Join Product and Category table");
        var result = await dbActions.LeftJoinProductWithCategoryNameAsync();
        result?.ShowItems();
    }

    public static async Task RightJoinProductAndCategory()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham" s 
        RIGHT JOIN "Category" c ON s."CategoryId" = c."Id"; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Right Join Product and Category table");
        var result = await dbActions.RightJoinProductWithCategoryNameAsync();
        result?.ShowItems();
    }

    public static async Task FullOuterJoinProductAndCategory()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham" s 
        FULL OUTER JOIN "Category" c ON s."CategoryId" = c."Id"; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Full Outer Join Product and Category table");
        var result = await dbActions.FullOuterJoinProductWithCategoryNameAsync();
        result?.ShowItems();
    }

    public static async Task Find2ProductWithPriceGreaterThan30WhichHaveTheGreatestPrice()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham"
        WHERE "SanPham"."Price"::numeric > 30
        ORDER BY "Price" DESC
        LIMIT 2; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Products with price greater than 30$:");
        var products = await dbActions.GetByConditionAsync<Product>(p => p.Price > 30, p => p.Price, orderByAscending: false, take: 2);
        products?.ShowItems();
    }

    public static async Task JoinProductAndCategory()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham" s
        JOIN "Category" c ON s."CategoryId" = c."Id"; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Join Product and Category table");
        var result = await dbActions.JoinAndGetByConditionAsync<Product, Category>(p => p.CategoryId, c => c.Id, JoinType.InnerJoin, take: 10, skip: 0, outerOrderByKeySelector: p => p.Price, outerOrderByAscending: false, innerOrderByKeySelector: c => c.Name, innerOrderByAscending: false);
        result?.ShowItems();
    }

    public static async Task Find2ProductWithPriceGreaterThan30WhichHaveTheLowestPrice()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham"
        WHERE "SanPham"."Price"::numeric > 30
        ORDER BY "Price" ASC
        LIMIT 2; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Products with price greater than 30$:");
        var products = await dbActions.GetByConditionAsync<Product>(p => p.Price > 30, p => p.Price, take: 2);
        products?.ShowItems();
    }

    public static async Task PrintAllProducts()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham"
        WHERE "SanPham"."Price"::numeric > 30
        ORDER BY "Price" ASC
        LIMIT 2; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Products with price greater than 30$:");
        var products = await dbActions.GetByConditionAsync<Product>(p => p.Price > 30, p => p.Price, take: 2);
        products?.ShowItems();
    }

    public static async Task FindProductById6()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham"
        WHERE "SanPham"."Id" = 6; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Products with id:6");
        var product = await dbActions.GetProductByIdAsync(6);
        new List<Product?> { product }?.ShowItems();
    }

    public static async Task FindTheNameOfTheFirstProductWithPriceGreaterThan100AndHasNameStartsWithLetterS()
    {
        /* // Postgresql: query
        SELECT title as "Name", "Price"
        FROM "SanPham"
        WHERE "SanPham"."Price"::numeric > 100

            AND "SanPham"."title" LIKE 'S%'
        LIMIT 1; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Products with price greater than 100$ and name starts with 'S':");
        var getFields = new List<string> { "Name" };
        var product = (await dbActions.GetByConditionAsync<Product>(p => p.Price > 100 && (p.Name ?? "").StartsWith("S"), take: 1, getFields: getFields))?.FirstOrDefault();
        new List<Product?> { product }?.ShowItems(showFields: getFields);
    }

    public static async Task PrintTheNameAndPriceOfAllProducts()
    {
        /* // Postgresql: query
        SELECT s.title as "Name", s."Price"
        FROM "SanPham" s; */
        var dbActions = new DBShopActions(_unitOfWork);
        System.Console.WriteLine($"Products with only name and price:");
        var getFields = new List<string> { "Name", "Price" };
        var products = await dbActions.GetByConditionAsync<Product>(getFields: getFields);
        products?.ShowItems(showFields: getFields);
    }

    public static async Task PrintTheNamePriceAndCategoryNameOfAllProducts()
    {
        /* // Postgresql: query
        SELECT *
        FROM "SanPham" sp 
        INNER JOIN "Category" c ON sp."CategoryId" = c."Id"; */
        var dbActions = new DBShopActions(_unitOfWork);
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
