using System.Linq.Expressions;
using EFC_Attributes;
using EFC_Interfaces;
using EFC_ModelContext;
using EFC_Repositories.Interfaces;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFC_Repositories;

public class Repository<T> : Repository, IRepository<T> where T : class, IEFModel
{
    public Repository(IUnitOfWork unitOfWork, params ShopContext[] shopContexts) : base(unitOfWork, shopContexts)
    {
    }

    public async Task AddAsync(params T[] records)
    {
        foreach (ShopContext context in _shopContexts)
        {
            await context.AddRangeAsync(records);
        }
    }

    public async Task DeleteAsync(int id)
    {
        foreach (ShopContext context in _shopContexts)
        {
            var dbSetProperty = context.GetGenericTypeOfDbSet<ShopContext, T>();
            if (dbSetProperty is null)
            {
                System.Console.WriteLine("Table does not exist");
                return;
            }
            var record = await dbSetProperty.FindAsync(id);
            if (record is null)
            {
                System.Console.WriteLine($"{typeof(T).Name} not found");
                return;
            }
            dbSetProperty.Remove(record);
        }
    }

    public async Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var context = _shopContexts[0];
        var dbSetProperty = context.GetGenericTypeOfDbSet<ShopContext, T>();
        if (dbSetProperty is null)
        {
            System.Console.WriteLine($"Table {typeof(T).Name} is empty");
            return null;
        }
        var records = await dbSetProperty.Where(predicate).ToListAsync();
        foreach (var record in records)
        {
            await record.LoadNavigationPropertiesValues(context);
        }
        records.ShowItems();
        return records;
    }

    public async Task<IEnumerable<T>?> GetAll()
    {
        var context = _shopContexts[0];
        var dbSetProperty = context.GetGenericTypeOfDbSet<ShopContext, T>();
        if (dbSetProperty is null)
        {
            System.Console.WriteLine($"Table {typeof(T).Name} is empty");
            return null;
        }
        var records = await dbSetProperty.ToListAsync();
        foreach (var record in records)
        {
            await record.LoadNavigationPropertiesValues(context);
        }
        records.ShowItems();
        return records;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var context = _shopContexts[0];
        var dbSetProperty = context.GetGenericTypeOfDbSet<ShopContext, T>();
        if (dbSetProperty is null)
        {
            System.Console.WriteLine($"Table {typeof(T).Name} does not exist");
            return null;
        }
        var record = await dbSetProperty.FindAsync(id);
        if (record is null)
        {
            System.Console.WriteLine($"{typeof(T).Name} with id {id} not found");
            return null;
        }

        var entityType = context.Model.FindEntityType(typeof(T)) ?? throw new NullReferenceException();
        await record.LoadNavigationPropertiesValues(context);
        new List<T?> { record }.ShowItems();
        return record;
    }

    public async Task UpdateAsync(int id, T entity)
    {
        foreach (ShopContext context in _shopContexts)
        {
            var dbSetProperty = context.GetGenericTypeOfDbSet<ShopContext, T>();
            if (dbSetProperty is null)
            {
                System.Console.WriteLine($"Table {typeof(T).Name} does not exist");
                return;
            }
            var recordFromDB = await dbSetProperty.FindAsync(id);
            if (recordFromDB is null)
            {
                System.Console.WriteLine($"{typeof(T).Name} with id {id} not found");
                return;
            }

            foreach (var property in context.Entry(recordFromDB).Properties.Where(p => p.Metadata.Name != "Id"))
            {
                var newValue = context.Entry(entity).Property(property.Metadata.Name).CurrentValue;
                var oldValue = property.CurrentValue;
                if (newValue is null || newValue.ToString() == "" || newValue.Equals(oldValue))
                {
                    continue;
                }
                property.CurrentValue = newValue;
                property.IsModified = true;
            }
        }
    }
}
