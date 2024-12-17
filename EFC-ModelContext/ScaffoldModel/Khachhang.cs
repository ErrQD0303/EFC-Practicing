using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFC_ModelContext.ScaffoldModel;

[Table("Khachhang")]
public partial class Khachhang
{
    [Key]
    [Column("KhachhangID")]
    public int KhachhangId { get; set; }

    [StringLength(255)]
    public string? HoTen { get; set; }

    [StringLength(255)]
    public string? TenLienLac { get; set; }

    [StringLength(255)]
    public string? Diachi { get; set; }

    [StringLength(255)]
    public string? Thanhpho { get; set; }

    [StringLength(255)]
    public string? MaBuudien { get; set; }

    [StringLength(255)]
    public string? QuocGia { get; set; }
}
