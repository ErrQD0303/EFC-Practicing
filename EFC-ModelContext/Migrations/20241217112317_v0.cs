using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFC_ModelContext.Migrations
{
    /// <inheritdoc />
    public partial class v0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cungcap",
                columns: table => new
                {
                    CungcapID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tendaydu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TenLienhe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Diachi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Thanhpho = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaBuudien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Quocgia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Dienthoai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cungcap__C6380F3D8189B72C", x => x.CungcapID);
                });

            migrationBuilder.CreateTable(
                name: "Danhmuc",
                columns: table => new
                {
                    DanhmucID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Danhmuc__15F7E73AF6910631", x => x.DanhmucID);
                });

            migrationBuilder.CreateTable(
                name: "Donhang",
                columns: table => new
                {
                    DonhangID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KhachhangID = table.Column<int>(type: "int", nullable: true),
                    NhanvienID = table.Column<int>(type: "int", nullable: true),
                    Ngaydathang = table.Column<DateOnly>(type: "date", nullable: true),
                    ShipperID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Donhang__99AA9CD5F1A1FFFD", x => x.DonhangID);
                });

            migrationBuilder.CreateTable(
                name: "DonhangChitiet",
                columns: table => new
                {
                    DonhangChitietID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonhangID = table.Column<int>(type: "int", nullable: true),
                    SanphamID = table.Column<int>(type: "int", nullable: true),
                    Soluong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonhangC__96D8B175242FF5AC", x => x.DonhangChitietID);
                });

            migrationBuilder.CreateTable(
                name: "Khachhang",
                columns: table => new
                {
                    KhachhangID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TenLienLac = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Diachi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Thanhpho = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaBuudien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    QuocGia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Khachhan__800808009F79E3CC", x => x.KhachhangID);
                });

            migrationBuilder.CreateTable(
                name: "Nhanvien",
                columns: table => new
                {
                    NhanvienID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Ho = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    Anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Ghichu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Nhanvien__92550447826C7F23", x => x.NhanvienID);
                });

            migrationBuilder.CreateTable(
                name: "Sanpham",
                columns: table => new
                {
                    SanphamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanpham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CungcapID = table.Column<int>(type: "int", nullable: true),
                    DanhmucID = table.Column<int>(type: "int", nullable: true),
                    Donvi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(13,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sanpham__BFF15FAC5C136BC2", x => x.SanphamID);
                });

            migrationBuilder.CreateTable(
                name: "Shippers",
                columns: table => new
                {
                    ShipperID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hoten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Sodienthoai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shippers__1F8AFFB9BE331A08", x => x.ShipperID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cungcap");

            migrationBuilder.DropTable(
                name: "Danhmuc");

            migrationBuilder.DropTable(
                name: "Donhang");

            migrationBuilder.DropTable(
                name: "DonhangChitiet");

            migrationBuilder.DropTable(
                name: "Khachhang");

            migrationBuilder.DropTable(
                name: "Nhanvien");

            migrationBuilder.DropTable(
                name: "Sanpham");

            migrationBuilder.DropTable(
                name: "Shippers");
        }
    }
}
