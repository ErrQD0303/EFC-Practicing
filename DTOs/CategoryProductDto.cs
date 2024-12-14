using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTOs
{
    public class CategoryProductDto
    {
        public int? ProductId { get; set; }
        public string? ProductName { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
        public int? CategoryId2 { get; set; }
        public string? CategoryName2 { get; set; } = string.Empty;
    }
}