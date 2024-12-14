using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC_Interfaces;
using EFC_Attributes;

namespace EFC_Models
{
    [Table("Category")]
    public class Category : IEFModel
    {
        [Key]
        public int Id { set; get; }

        [StringLength(100)]
        public string? Name { set; get; }

        [DBTypes("Category", "Description")]
        public string? Description { set; get; }

        public List<Product> Products { get; set; } = [];

        public int CategoryDetailId { get; set; }
        public CategoryDetail? CategoryDetail { get; set; }
    }
}