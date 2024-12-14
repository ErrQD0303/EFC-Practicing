using System.Linq.Expressions;
using EFC_Attributes;
using EFC_Interfaces;
using EFC_Models;
using EFC_Repositories;
using EFC_Repositories.Interfaces;
using Helpers;
using Microsoft.EntityFrameworkCore;

namespace EFC_DBActions;

public class DBShopActions : IDBActions
{
    IUnitOfWork _unitOfWork;

    public DBShopActions(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateDatabase()
    {
        await _unitOfWork.Repository.CreateDatabase();
    }

    public async Task DeleteDatabase()
    {
        await _unitOfWork.Repository.DeleteDatabase();
    }

    public async Task AddAsync<TEntity>(params IEnumerable<TEntity> records) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        await entityRepository.AddAsync(records.ToArray());
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync<TEntity>() where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        return await entityRepository.GetAll();
    }

    public async Task<TEntity?> GetByIdAsync<TEntity>(int id) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        return await entityRepository.GetByIdAsync(id);
    }
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await GetByIdAsync<Product>(id);
    }
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await GetByIdAsync<Category>(id);
    }

    public async Task<IEnumerable<TEntity>?> GetByConditionAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = null) where TEntity : class, IEFModel, new()
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        if (predicate is null) return await entityRepository.GetAll();
        return await entityRepository.FindAsync(predicate);
    }

    public async Task<IEnumerable<Product>?> GetProductByName(string name)
    {
        return await _unitOfWork.Products.FindAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<Product>?> GetProductByProvider(string provider)
    {
        return await _unitOfWork.Products.FindAsync(p => p.Provider == provider);
    }

    public async Task<IEnumerable<TEntity>?> GetByConditionAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, decimal>>? orderByKeySelector = null, bool orderByAscending = true, int? take = null, int? skip = null, IEnumerable<string>? getFields = null) where TEntity : class, IEFModel, new()
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        var queryableObj = await entityRepository.GetAsQueryableAsync();

        if (predicate is not null)
            queryableObj = queryableObj.Where(predicate);

        if (orderByKeySelector is not null)
        {
            if (orderByAscending)
            {
                queryableObj = queryableObj.OrderBy(orderByKeySelector);
            }
            else
            {
                queryableObj = queryableObj.OrderByDescending(orderByKeySelector);
            }
        }

        if (skip.HasValue)
        {
            queryableObj = queryableObj.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            queryableObj = queryableObj.Take(take.Value);
        }

        if (getFields is not null && getFields.Any())
        {
            /* var entityType = typeof(TEntity);
            queryableObj = queryableObj.Select(p =>
            {
                var obj = new TEntity();
                foreach (var field in getFields)
                {
                    var prop = entityType.GetProperty(field);
                    if (prop != null)
                    {
                        prop.SetValue(obj, prop.GetValue(p));
                    }
                }
                return obj;
            }); */
            var parameter = Expression.Parameter(typeof(TEntity), "x"); // x
            var bindings = getFields.Select(field =>
            {
                var property = typeof(TEntity).GetProperty(field) ?? throw new ArgumentNullException($"Property {field} does not exist on type {typeof(TEntity).Name}");

                return Expression.Bind(property, Expression.Property(parameter, property)); // Property = x.Property 
            }); //  { Property1 = x.Property1, Property2 = x.Property2, ... }

            var body = Expression.MemberInit(Expression.New(typeof(TEntity)), bindings); // new TEntity { Property1 = x.Property1, Property2 = x.Property2, ... }
            var lambda = Expression.Lambda<Func<TEntity, TEntity>>(body, parameter); // accept a TEntity and return a new TEntity

            queryableObj = queryableObj.Select(lambda); // Use the new lambda on the queryable object
        }

        return await queryableObj.ToListAsync();
    }

    public async Task<IEnumerable<object>?> JoinAndGetByConditionAsync<TEntity, TJoinEntity>(Expression<Func<TEntity, int?>> outerKeySelector, Expression<Func<TJoinEntity, int?>> innerKeySelector, JoinType joinType, Expression<Func<TEntity, bool>>? outerPredicate = null, Expression<Func<TJoinEntity, bool>>? innerPredicate = null, Expression<Func<TEntity, object>>? outerOrderByKeySelector = null, bool outerOrderByAscending = true, Expression<Func<TJoinEntity, object>>? innerOrderByKeySelector = null, bool innerOrderByAscending = true, int? take = null, int? skip = null, IEnumerable<string>? outerGetFields = null, IEnumerable<string>? innerGetFields = null)
        where TEntity : class, IEFModel, new()
        where TJoinEntity : class, IEFModel, new()
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        var queryableEntity = await entityRepository.GetAsQueryableAsync();
        var joinEntityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TJoinEntity>() as IRepository<TJoinEntity> ?? throw new NullReferenceException();
        var queryableJoinEntity = await joinEntityRepository.GetAsQueryableAsync();

        if (outerOrderByKeySelector is not null)
        {
            if (outerOrderByAscending)
            {
                queryableEntity = queryableEntity.OrderBy(outerOrderByKeySelector);
            }
            else
            {
                queryableEntity = queryableEntity.OrderByDescending(outerOrderByKeySelector);
            }
        }

        if (innerOrderByKeySelector is not null)
        {
            if (innerOrderByAscending)
            {
                queryableJoinEntity = queryableJoinEntity.OrderBy(innerOrderByKeySelector);
            }
            else
            {
                queryableJoinEntity = queryableJoinEntity.OrderByDescending(innerOrderByKeySelector);
            }
        }

        IQueryable<object>? queryableResult = null;
        if (joinType == JoinType.InnerJoin)
        {
            queryableResult = queryableEntity.Join(
                queryableJoinEntity,
                outerKeySelector,
                innerKeySelector,
                (entity, joinEntity) => new
                {
                    LHSEntity = entity,
                    RHSEntity = joinEntity
                });
        }
        /* var queryableObj = queryableEntity.GroupJoin(
            queryableJoinEntity,
            outerKeySelector,
            innerKeySelector,
            (entity, joinEntity) => new
            {
                LHSEntity = entity,
                RHSEntity = joinEntity
            });

        LambdaExpression? lambda = null;
        if (joinType != JoinType.InnerJoin)
        {
            if (joinType == JoinType.LeftJoin)
            {
                var parameter1 = Expression.Parameter(typeof(object), "x"); // x

                var parameter2 = Expression.Parameter(typeof(TJoinEntity), "RHS"); // RHS


                // Extract left entity (TEntity) and right entity (TJoinEntity) from parameter1
                var lhs = Expression.Property(parameter1, "LHSEntity");
                var rhsCollection = Expression.Property(parameter1, "RHSEntity");

                IEnumerable<MemberAssignment>? outerFieldsBinding = null;
                if (outerGetFields is not null)
                {
                    outerFieldsBinding = typeof(TEntity).GetProperties()
                        .Where(p => outerGetFields.Any(f => string.Equals(f, p.Name, StringComparison.OrdinalIgnoreCase)))
                        .ToList()
                        .Select(p => Expression.Bind(p, Expression.Property(parameter1, p)))
                        .ToList();
                }

                IEnumerable<MemberAssignment>? innerFieldsBinding = null;
                if (innerGetFields is not null)
                {
                    innerFieldsBinding = typeof(TJoinEntity).GetProperties()
                        .Where(p => innerGetFields.Any(f => string.Equals(f, p.Name, StringComparison.OrdinalIgnoreCase)))
                        .ToList()
                        .Select(p => Expression.Bind(p, Expression.Property(parameter2, p)))
                        .ToList();
                }

                var bindings = outerFieldsBinding?.Concat(innerFieldsBinding ?? Enumerable.Empty<MemberAssignment>()) ?? innerFieldsBinding ?? Enumerable.Empty<MemberAssignment>();

                var body = Expression.MemberInit(Expression.New(typeof(object)), bindings.ToArray());

                lambda = Expression.Lambda<Func<TEntity, TJoinEntity, object>>(body, parameter1, parameter2);

                // Create the SelectMany method call, ensuring we handle the left join logic
                var methodCall = Expression.Call(
                typeof(Queryable),
                "SelectMany",
                new Type[] { typeof(object), typeof(TJoinEntity), typeof(object) },
                queryableObj.Expression,
                Expression.Lambda(
                    Expression.Call(
                        typeof(Enumerable),
                        "DefaultIfEmpty",
                        new Type[] { typeof(TJoinEntity) },
                        rhsCollection
                    ),
                    parameter1
                ),
                lambda
                );

                // Execute the final query
            }
        } */

        if (queryableResult is null)
        {
            throw new NullReferenceException();
        }

        if (outerPredicate is not null)
            queryableResult = queryableResult.Where(x => outerPredicate.Compile().Invoke(x.GetType().GetProperty("LHSEntity").GetValue(x) as TEntity));

        if (innerPredicate is not null)
            queryableResult = queryableResult.Where(x => innerPredicate.Compile().Invoke(x.GetType().GetProperty("RHSEntity").GetValue(x) as TJoinEntity));


        if (skip.HasValue)
        {
            queryableResult = queryableResult.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            queryableResult = queryableResult.Take(take.Value);
        }

        return await queryableResult.ToListAsync();
    }

    public async Task<IEnumerable<object>?> LeftJoinProductWithCategoryNameAsync()
    {
        var productQueryable = await _unitOfWork.Products.GetAsQueryableAsync();
        var categoryQueryable = await _unitOfWork.Categories.GetAsQueryableAsync();

        return await productQueryable.GroupJoin(
                categoryQueryable,
                p => p.CategoryId,
                c => c.Id,
                (product, category) => new
                {
                    Product = product,
                    Category = category
                }
            )
            .SelectMany(
                x => x.Category.DefaultIfEmpty(),
                (x, c) => new
                {
                    x.Product.Id,
                    x.Product.Name,
                    x.Product.Provider,
                    x.Product.Price,
                    CategoryName = c!.Name ?? "",
                    x.Product.CategoryId2,
                    x.Product.UserPostId
                }
            )
            .GroupJoin(
                categoryQueryable,
                p => p.CategoryId2,
                c => c.Id,
                (product, category) => new
                {
                    Product = product,
                    Category = category
                }
            )
            .SelectMany(
                x => x.Category.DefaultIfEmpty(),
                (x, c) => new
                {
                    Id = x.Product.Id,
                    Name = x.Product.Name,
                    Provider = x.Product.Provider,
                    Price = x.Product.Price,
                    CategoryName = x.Product.CategoryName,
                    Category2Name = c!.Name ?? "",
                    UserPostId = x.Product.UserPostId
                }
            ).ToListAsync();
    }

    public async Task<IEnumerable<object>?> RightJoinProductWithCategoryNameAsync()
    {
        var productQueryable = await _unitOfWork.Products.GetAsQueryableAsync();
        var categoryQueryable = await _unitOfWork.Categories.GetAsQueryableAsync();

        return await categoryQueryable.GroupJoin(
                productQueryable,
                c => c.Id,
                p => p.CategoryId,
                (category, product) => new
                {
                    Category = category,
                    Product = product,
                }
            )
            .SelectMany(
                x => x.Product.DefaultIfEmpty(),
                (x, p) => new
                {
                    Id = (p == null) ? -1 : p.Id,
                    Name = (p == null) ? "null" : p.Name,
                    Provider = (p == null) ? "null" : p.Provider,
                    Price = (p == null) ? -1 : p.Price,
                    CategoryName = x.Category.Name,
                    CategoryId2 = (p == null) ? null : p.CategoryId2,
                    UserPostId = (p == null) ? null : p.UserPostId
                }
            )
            .ToListAsync();
    }

    public async Task<IEnumerable<object>?> FullOuterJoinProductWithCategoryNameAsync()
    {
        var productQueryable = await _unitOfWork.Products.GetAsQueryableAsync();
        var categoryQueryable = await _unitOfWork.Categories.GetAsQueryableAsync();

        return await productQueryable.GroupJoin(
                categoryQueryable,
                p => p.CategoryId,
                c => c.Id,
                (product, category) => new
                {
                    Product = product,
                    Category = category
                }
            )
            .SelectMany(
                x => x.Category.DefaultIfEmpty(),
                (x, c) => new
                {
                    Id = x.Product.Id,
                    Name = x.Product.Name,
                    Provider = x.Product.Provider,
                    Price = x.Product.Price,
                    CategoryName = c!.Name ?? "",
                    CategoryId2 = x.Product.CategoryId2,
                    UserPostId = x.Product.UserPostId
                }
            ).Union(
                categoryQueryable.GroupJoin(
                    productQueryable,
                    c => c.Id,
                    p => p.CategoryId,
                    (category, product) => new
                    {
                        Category = category,
                        Product = product,
                    }
                )
                .SelectMany(
                    x => x.Product.DefaultIfEmpty(),
                    (x, p) => new
                    {
                        Id = (p == null) ? -1 : p.Id,
                        Name = (p == null) ? "null" : p.Name,
                        Provider = (p == null) ? "null" : p.Provider,
                        Price = (p == null) ? -1 : p.Price,
                        CategoryName = x.Category.Name,
                        CategoryId2 = (p == null) ? null : p.CategoryId2,
                        UserPostId = (p == null) ? null : p.UserPostId
                    }
                )
            ).ToListAsync();
    }

    public async Task<TEntity?> UpdateAsync<TEntity>(int id, TEntity record) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        await entityRepository.UpdateAsync(id, record);
        await _unitOfWork.SaveChangesAsync();
        System.Console.WriteLine($"{typeof(TEntity).Name} updated");

        var returnRecord = await entityRepository.GetByIdAsync(id);
        new List<TEntity?>() { returnRecord }.ShowItems();

        return returnRecord;
    }
    public async Task<Product?> UpdateName(int id, string name)
    {
        var product = await GetByIdAsync<Product>(id);
        if (product is null)
        {
            System.Console.WriteLine("Product not found");
            return null;
        }
        return await UpdateAsync(id, new Product { Name = name, CategoryId = product.CategoryId });
    }
    public async Task<Product?> UpdateProvider(int id, string provider)
    {
        var product = await GetByIdAsync<Product>(id);
        if (product is null)
        {
            System.Console.WriteLine("Product not found");
            return null;
        }
        return await UpdateAsync(id, new Product { Provider = provider, CategoryId = product.CategoryId });
    }

    public async Task DeleteAsync<TEntity>(int id) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        await entityRepository.DeleteAsync(id);
        await entityRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>?> FromSQLRawAsync<TEntity>(string? sql) where TEntity : class
    {
        return await ((IDBActions)this).FromSQLRawAsync<TEntity>(sql);
    }


    async Task<IEnumerable<TEntity>?> IDBActions.FromSQLRawAsync<TEntity>(string? sql)
    {
        if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

        return await _unitOfWork.Repository.FromSqlRaw<TEntity>(sql);
    }
}
