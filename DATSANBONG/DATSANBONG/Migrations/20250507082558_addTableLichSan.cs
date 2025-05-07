using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATSANBONG.Migrations
{
    /// <inheritdoc />
    public partial class addTableLichSan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LichSans",
                columns: table => new
                {
                    MaLichSan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanCon = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    MaSanBong = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    thu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioBatDau = table.Column<TimeSpan>(type: "time", nullable: false),
                    GioKetThuc = table.Column<TimeSpan>(type: "time", nullable: false),
                    GiaThue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSans", x => x.MaLichSan);
                    table.ForeignKey(
                        name: "FK_LichSans_SanBongs_MaSanBong",
                        column: x => x.MaSanBong,
                        principalTable: "SanBongs",
                        principalColumn: "MaSanBong",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSans_chiTietSanBongs_MaSanBong_MaSanCon",
                        columns: x => new { x.MaSanBong, x.MaSanCon },
                        principalTable: "chiTietSanBongs",
                        principalColumns: new[] { "MaSanBong", "MaSanCon" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichSans_MaSanBong_MaSanCon",
                table: "LichSans",
                columns: new[] { "MaSanBong", "MaSanCon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichSans");
        }
    }
}
