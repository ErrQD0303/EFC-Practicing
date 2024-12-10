using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC_Interfaces;

namespace EFC_Models;

[Table("Products")]
public class Product : IEFModel
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(50)]
    public string? Name { get; set; }

    [StringLength(50)]
    public string? Provider { get; set; }
}
