using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATSANBONG.Migrations
{
    /// <inheritdoc />
    public partial class addtableDONDATSANandCHITIETDONDATSAN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChiTietSanBongMaSanBong",
                table: "LichSans",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChiTietSanBongMaSanCon",
                table: "LichSans",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SanBongMaSanBong",
                table: "LichSans",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DonDatSans",
                columns: table => new
                {
                    MaDatSan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoLuongSan = table.Column<int>(type: "int", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PhuongThucTT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrangThaiTT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonDatSans", x => x.MaDatSan);
                    table.ForeignKey(
                        name: "FK_DonDatSans_AspNetUsers_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonDatSans_NhanViens_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonDatSans",
                columns: table => new
                {
                    MaDatSan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanBong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanCon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GioBatDau = table.Column<TimeSpan>(type: "time", nullable: false),
                    GioKetThuc = table.Column<TimeSpan>(type: "time", nullable: false),
                    ChiTietSanBongMaSanBong = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    ChiTietSanBongMaSanCon = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonDatSans", x => new { x.MaDatSan, x.MaSanCon, x.MaSanBong, x.GioBatDau });
                    table.ForeignKey(
                        name: "FK_ChiTietDonDatSans_DonDatSans_MaDatSan",
                        column: x => x.MaDatSan,
                        principalTable: "DonDatSans",
                        principalColumn: "MaDatSan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonDatSans_chiTietSanBongs_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                        columns: x => new { x.ChiTietSanBongMaSanBong, x.ChiTietSanBongMaSanCon },
                        principalTable: "chiTietSanBongs",
                        principalColumns: new[] { "MaSanBong", "MaSanCon" });
                    table.ForeignKey(
                        name: "FK_ChiTietDonDatSans_chiTietSanBongs_MaSanBong_MaSanCon",
                        columns: x => new { x.MaSanBong, x.MaSanCon },
                        principalTable: "chiTietSanBongs",
                        principalColumns: new[] { "MaSanBong", "MaSanCon" },
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichSans_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LichSans",
                columns: new[] { "ChiTietSanBongMaSanBong", "ChiTietSanBongMaSanCon" });

            migrationBuilder.CreateIndex(
                name: "IX_LichSans_SanBongMaSanBong",
                table: "LichSans",
                column: "SanBongMaSanBong");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonDatSans_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "ChiTietDonDatSans",
                columns: new[] { "ChiTietSanBongMaSanBong", "ChiTietSanBongMaSanCon" });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonDatSans_MaSanBong_MaSanCon",
                table: "ChiTietDonDatSans",
                columns: new[] { "MaSanBong", "MaSanCon" });

            migrationBuilder.CreateIndex(
                name: "IX_DonDatSans_MaKhachHang",
                table: "DonDatSans",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonDatSans_MaNhanVien",
                table: "DonDatSans",
                column: "MaNhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK_LichSans_SanBongs_SanBongMaSanBong",
                table: "LichSans",
                column: "SanBongMaSanBong",
                principalTable: "SanBongs",
                principalColumn: "MaSanBong");

            migrationBuilder.AddForeignKey(
                name: "FK_LichSans_chiTietSanBongs_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LichSans",
                columns: new[] { "ChiTietSanBongMaSanBong", "ChiTietSanBongMaSanCon" },
                principalTable: "chiTietSanBongs",
                principalColumns: new[] { "MaSanBong", "MaSanCon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LichSans_SanBongs_SanBongMaSanBong",
                table: "LichSans");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSans_chiTietSanBongs_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LichSans");

            migrationBuilder.DropTable(
                name: "ChiTietDonDatSans");

            migrationBuilder.DropTable(
                name: "DonDatSans");

            migrationBuilder.DropIndex(
                name: "IX_LichSans_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LichSans");

            migrationBuilder.DropIndex(
                name: "IX_LichSans_SanBongMaSanBong",
                table: "LichSans");

            migrationBuilder.DropColumn(
                name: "ChiTietSanBongMaSanBong",
                table: "LichSans");

            migrationBuilder.DropColumn(
                name: "ChiTietSanBongMaSanCon",
                table: "LichSans");

            migrationBuilder.DropColumn(
                name: "SanBongMaSanBong",
                table: "LichSans");
        }
    }
}
