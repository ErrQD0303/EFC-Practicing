using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EFC_Interfaces;

namespace EFC_Models
{
    [Table("Category")]
    public class Category : IEFModel
    {
        [Key]
        public int CategoryId { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}