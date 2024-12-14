using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using EFC_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Helpers;

public static class Helper
{
    public static void ShowItems(this IEnumerable<object?> items, IEnumerable<string>? showFields = null)
    {
        var sb = new StringBuilder();
        var type = items.FirstOrDefault()?.GetType();
        if (type == null)
        {
            sb.AppendLine("No records found.");
            System.Console.WriteLine(sb.ToString());
            return;
        }

        showFields ??= Enumerable.Empty<string>();
        var properties = type.GetProperties();
        var typeName = type.Name;
        sb.AppendLine($"{typeName}:");
        foreach (var property in properties.Where(p => showFields.Contains(p.Name, StringComparer.OrdinalIgnoreCase) || !showFields.Any()))
        {
            if (property.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append($"{property.Name,-5}");
                continue;
            }
            sb.Append($"{property.Name,-20}");
        }
        sb.AppendLine();

        if (!items.Any())
        {
            sb.AppendLine("No records found.");
        }
        else
        {
            foreach (var item in items)
            {
                foreach (var prop in properties.Where(p => showFields.Contains(p.Name, StringComparer.OrdinalIgnoreCase) || !showFields.Any()))
                {
                    var value = prop.GetValue(item)?.ToString() ?? "null";
                    if (prop.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        sb.Append($"{value,-5}");
                        continue;
                    }
                    sb.Append($"{value,-20}");
                }
                sb.AppendLine();
            }
        }
        sb.AppendLine("\n");
        System.Console.WriteLine(sb.ToString());
    }

    public static object? GetUnitOfWorkProperty<TUnitOfWork, TRepositoryTableAttribute, TEntity>(this TUnitOfWork uow)
    {
        var iUOWType = typeof(TUnitOfWork);
        var iUOWProperties = iUOWType.GetProperties();
        var propertiesWithRepositoryTableAttr = iUOWProperties.Where(p => p.CustomAttributes.Any(a => a.AttributeType.Name == typeof(TRepositoryTableAttribute).Name));
        var propertyHasTheSameTypeAsEntityType = propertiesWithRepositoryTableAttr
            .FirstOrDefault(p =>
            {
                var attribute = Convert.ChangeType(p.GetCustomAttribute(typeof(TRepositoryTableAttribute)), typeof(TRepositoryTableAttribute));
                return attribute != null && attribute.GetType().GetProperty("TableName")?.GetValue(attribute)?.ToString() == typeof(TEntity).Name;
            }) ?? throw new ArgumentException("Table does not exist");
        return propertyHasTheSameTypeAsEntityType.GetValue(uow);
    }

    public static DbSet<TModel>? GetGenericTypeOfDbSet<TContext, TModel>(this TContext shopContext) where TModel : class, IEFModel
    {
        var propertyWithRepositoryAttr = typeof(TContext).GetProperties().FirstOrDefault(p => p.PropertyType == typeof(DbSet<TModel>) && string.Equals(p.PropertyType.GetGenericArguments().FirstOrDefault()?.Name ?? "", typeof(TModel).Name, StringComparison.OrdinalIgnoreCase));
        return propertyWithRepositoryAttr?.GetValue(shopContext) as DbSet<TModel>;
    }

    public static TDBTypesAttribute? GetDBTypesAttributes<TDBTypesAttribute>(this PropertyInfo property) where TDBTypesAttribute : Attribute
    {
        return property.GetCustomAttribute<TDBTypesAttribute>();
    }

    public static async Task LoadNavigationPropertiesValues<T, TContext>(this T record, TContext context) where TContext : DbContext
    {
        if (record is null)
        {
            return;
        }

        var entityType = context.Model.FindEntityType(typeof(T)) ?? throw new NullReferenceException();
        foreach (var navigation in entityType.GetNavigations())
        {
            var navigationProperty = context.Entry(record).Navigation(navigation.Name);
            if (navigationProperty is null)
            {
                continue;
            }
            await navigationProperty.LoadAsync();
        }
    }
}
