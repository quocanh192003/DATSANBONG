using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATSANBONG.Migrations
{
    /// <inheritdoc />
    public partial class addTableHinhAnh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "SanBongs");

            migrationBuilder.CreateTable(
                name: "HinhAnhs",
                columns: table => new
                {
                    maHinhAnh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    maSanBong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    urlHinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhs", x => x.maHinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhs_SanBongs_maSanBong",
                        column: x => x.maSanBong,
                        principalTable: "SanBongs",
                        principalColumn: "MaSanBong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhs_maSanBong",
                table: "HinhAnhs",
                column: "maSanBong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhAnhs");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "SanBongs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
