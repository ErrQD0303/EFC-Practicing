using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EFC_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFC_Models
{
    [Table("User")]
    [Comment("User Table")]
    [Index(nameof(UserName), IsUnique = true)]
    public class User : IEFModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Equivalent to UseIdentityColumn(1, 1) in Fluent API, if you want to change the seed and increment value, you can use the Fluent API
        public int Id { get; set; }

        [StringLength(100)]
        [Column("user_name")]
        [DefaultValue("Không tên")]
        [MaxLength(20)]
        public string? UserName { get; set; }

        public List<Product> ProductsPost { set; get; } = [];
    }
}