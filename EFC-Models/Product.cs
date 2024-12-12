using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC_Attributes;
using EFC_Interfaces;

namespace EFC_Models;

[Table("Product")]
public class Product : IEFModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? Name { set; get; }

    [StringLength(100)] public string? Provider { set; get; }

    [DBTypes("Product", "Price")]
    public decimal Price { set; get; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { set; get; }

    public int? CategoryId2 { get; set; }

    [ForeignKey("CategoryId2")]
    [InverseProperty("Products")]
    public Category? Category2 { set; get; }
}
