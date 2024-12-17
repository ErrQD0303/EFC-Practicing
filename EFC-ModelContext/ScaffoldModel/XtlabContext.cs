using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFC_ModelContext.ScaffoldModel;

public partial class XtlabContext : DbContext
{
    public XtlabContext()
    {
    }

    public XtlabContext(DbContextOptions<XtlabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cungcap> Cungcaps { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<DonhangChitiet> DonhangChitiets { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=172.26.144.55,1433;Initial Catalog=xtlab;User ID=sa;Password=Admin@123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cungcap>(entity =>
        {
            entity.HasKey(e => e.CungcapId).HasName("PK__Cungcap__C6380F3D8189B72C");
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.DanhmucId).HasName("PK__Danhmuc__15F7E73AF6910631");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.DonhangId).HasName("PK__Donhang__99AA9CD5F1A1FFFD");
        });

        modelBuilder.Entity<DonhangChitiet>(entity =>
        {
            entity.HasKey(e => e.DonhangChitietId).HasName("PK__DonhangC__96D8B175242FF5AC");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.KhachhangId).HasName("PK__Khachhan__800808009F79E3CC");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.NhanvienId).HasName("PK__Nhanvien__92550447826C7F23");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.SanphamId).HasName("PK__Sanpham__BFF15FAC5C136BC2");
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.HasKey(e => e.ShipperId).HasName("PK__Shippers__1F8AFFB9BE331A08");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
