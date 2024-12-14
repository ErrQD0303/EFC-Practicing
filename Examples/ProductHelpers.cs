using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFC_Models;

namespace Examples
{
    public static class ProductHelper
    {
        public static void ShowItems(this IEnumerable<Product?> items, IEnumerable<string>? showFields = null)
        {
            showFields ??= Enumerable.Empty<string>();
            var sb = new StringBuilder();
            var type = items.FirstOrDefault()?.GetType();
            if (type == null)
            {
                sb.AppendLine("No records found.");
                System.Console.WriteLine(sb.ToString());
                return;
            }

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
                        var value = prop.GetValue(item);
                        if (prop.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                        {
                            sb.Append($"{value,-5}");
                            continue;
                        }
                        if (prop.Name.Equals("category", StringComparison.OrdinalIgnoreCase) || prop.Name.Equals("category2", StringComparison.OrdinalIgnoreCase))
                        {
                            var categoryName = (value as Category)?.Name;
                            sb.Append($"{categoryName ?? "",-20}");
                            continue;
                        }
                        sb.Append($"{value?.ToString() ?? "null",-20}");
                    }
                    sb.AppendLine();
                }
            }
            sb.AppendLine("\n");
            System.Console.WriteLine(sb.ToString());
        }
    }
}