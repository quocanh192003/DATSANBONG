using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATSANBONG.Migrations
{
    /// <inheritdoc />
    public partial class addTableSanBongAndNhanVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SanBongs",
                columns: table => new
                {
                    MaSanBong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenSanBong = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SoLuongSan = table.Column<int>(type: "int", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaChuSan = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanBongs", x => x.MaSanBong);
                    table.ForeignKey(
                        name: "FK_SanBongs_AspNetUsers_MaChuSan",
                        column: x => x.MaChuSan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MaChuSan = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MaSanBong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNhanVien);
                    table.ForeignKey(
                        name: "FK_NhanViens_AspNetUsers_MaChuSan",
                        column: x => x.MaChuSan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_NhanViens_AspNetUsers_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_NhanViens_SanBongs_MaSanBong",
                        column: x => x.MaSanBong,
                        principalTable: "SanBongs",
                        principalColumn: "MaSanBong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_MaChuSan",
                table: "NhanViens",
                column: "MaChuSan");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_MaSanBong",
                table: "NhanViens",
                column: "MaSanBong");

            migrationBuilder.CreateIndex(
                name: "IX_SanBongs_MaChuSan",
                table: "SanBongs",
                column: "MaChuSan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "SanBongs");
        }
    }
}
