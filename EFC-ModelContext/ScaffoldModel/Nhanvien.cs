using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFC_ModelContext.ScaffoldModel;

[Table("Nhanvien")]
public partial class Nhanvien
{
    [Key]
    [Column("NhanvienID")]
    public int NhanvienId { get; set; }

    [StringLength(255)]
    public string? Ten { get; set; }

    [StringLength(255)]
    public string? Ho { get; set; }

    public DateOnly? NgaySinh { get; set; }

    [StringLength(255)]
    public string? Anh { get; set; }

    [Column(TypeName = "text")]
    public string? Ghichu { get; set; }
}
