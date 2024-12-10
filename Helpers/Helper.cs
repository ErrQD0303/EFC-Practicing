using System.Text;
using EFC_Models;

namespace Helpers;

public static class Helper
{
    public static void ShowProducts(this IEnumerable<Product?> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Products:");
        sb.AppendLine($"{"ID",-5} {"Name",-20} {"Provider",-20}");
        if (products.Count() == 0)
        {
            sb.AppendLine("No records found.");
        }
        foreach (var product in products)
        {
            sb.AppendLine($"{product?.ProductId,-5} {product?.Name,-20} {product?.Provider,-20}");
        }
        sb.AppendLine("\n");
        System.Console.WriteLine(sb.ToString());
    }
}
