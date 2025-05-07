using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATSANBONG.Migrations
{
    /// <inheritdoc />
    public partial class addTableChitietsanbong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chiTietSanBongs",
                columns: table => new
                {
                    MaSanBong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanCon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenSanCon = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LoaiSanCon = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TrangThaiSan = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chiTietSanBongs", x => new { x.MaSanBong, x.MaSanCon });
                    table.ForeignKey(
                        name: "FK_chiTietSanBongs_SanBongs_MaSanBong",
                        column: x => x.MaSanBong,
                        principalTable: "SanBongs",
                        principalColumn: "MaSanBong",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chiTietSanBongs");
        }
    }
}
