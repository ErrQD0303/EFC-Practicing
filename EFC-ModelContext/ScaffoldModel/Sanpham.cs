using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFC_ModelContext.ScaffoldModel;

[Table("Sanpham")]
public partial class Sanpham
{
    [Key]
    [Column("SanphamID")]
    public int SanphamId { get; set; }

    [StringLength(255)]
    public string? TenSanpham { get; set; }

    [Column("CungcapID")]
    public int? CungcapId { get; set; }

    [Column("DanhmucID")]
    public int? DanhmucId { get; set; }

    [StringLength(255)]
    public string? Donvi { get; set; }

    [Column(TypeName = "decimal(13, 2)")]
    public decimal? Gia { get; set; }
}
