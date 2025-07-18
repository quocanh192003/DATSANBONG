﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATSANBONG.Migrations
{
    /// <inheritdoc />
    public partial class addThutoCHITIETDONDATSAN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thu",
                table: "DonDatSans");

            migrationBuilder.AddColumn<string>(
                name: "thu",
                table: "ChiTietDonDatSans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thu",
                table: "ChiTietDonDatSans");

            migrationBuilder.AddColumn<string>(
                name: "thu",
                table: "DonDatSans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
