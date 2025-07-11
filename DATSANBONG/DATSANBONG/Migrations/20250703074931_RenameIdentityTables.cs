using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATSANBONG.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonDatSans_DonDatSans_MaDatSan",
                table: "ChiTietDonDatSans");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonDatSans_chiTietSanBongs_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "ChiTietDonDatSans");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonDatSans_chiTietSanBongs_MaSanBong_MaSanCon",
                table: "ChiTietDonDatSans");

            migrationBuilder.DropForeignKey(
                name: "FK_chiTietSanBongs_SanBongs_MaSanBong",
                table: "chiTietSanBongs");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_AspNetUsers_MaNguoiDung",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_SanBongs_MaSanBong",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonDatSans_AspNetUsers_MaKhachHang",
                table: "DonDatSans");

            migrationBuilder.DropForeignKey(
                name: "FK_DonDatSans_NhanViens_MaNhanVien",
                table: "DonDatSans");

            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnhs_SanBongs_maSanBong",
                table: "HinhAnhs");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSans_SanBongs_MaSanBong",
                table: "LichSans");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSans_SanBongs_SanBongMaSanBong",
                table: "LichSans");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSans_chiTietSanBongs_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LichSans");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSans_chiTietSanBongs_MaSanBong_MaSanCon",
                table: "LichSans");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_AspNetUsers_MaChuSan",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_AspNetUsers_MaNhanVien",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_SanBongs_MaSanBong",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_SanBongs_AspNetUsers_MaChuSan",
                table: "SanBongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanBongs",
                table: "SanBongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhanViens",
                table: "NhanViens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LichSans",
                table: "LichSans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HinhAnhs",
                table: "HinhAnhs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonDatSans",
                table: "DonDatSans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DanhGias",
                table: "DanhGias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chiTietSanBongs",
                table: "chiTietSanBongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDonDatSans",
                table: "ChiTietDonDatSans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "SanBongs",
                newName: "SANBONG");

            migrationBuilder.RenameTable(
                name: "NhanViens",
                newName: "NHANVIEN");

            migrationBuilder.RenameTable(
                name: "LichSans",
                newName: "LICHSAN");

            migrationBuilder.RenameTable(
                name: "HinhAnhs",
                newName: "HINHANH");

            migrationBuilder.RenameTable(
                name: "DonDatSans",
                newName: "DONDATSAN");

            migrationBuilder.RenameTable(
                name: "DanhGias",
                newName: "DANHGIA");

            migrationBuilder.RenameTable(
                name: "chiTietSanBongs",
                newName: "CHITIETSANBONG");

            migrationBuilder.RenameTable(
                name: "ChiTietDonDatSans",
                newName: "CHITIETDONDATSAN");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "NGUOIDUNG");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "VAITRONGUOIDUNG");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "VAITRO");

            migrationBuilder.RenameIndex(
                name: "IX_SanBongs_MaChuSan",
                table: "SANBONG",
                newName: "IX_SANBONG_MaChuSan");

            migrationBuilder.RenameIndex(
                name: "IX_NhanViens_MaSanBong",
                table: "NHANVIEN",
                newName: "IX_NHANVIEN_MaSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_NhanViens_MaChuSan",
                table: "NHANVIEN",
                newName: "IX_NHANVIEN_MaChuSan");

            migrationBuilder.RenameIndex(
                name: "IX_LichSans_SanBongMaSanBong",
                table: "LICHSAN",
                newName: "IX_LICHSAN_SanBongMaSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_LichSans_MaSanBong_MaSanCon",
                table: "LICHSAN",
                newName: "IX_LICHSAN_MaSanBong_MaSanCon");

            migrationBuilder.RenameIndex(
                name: "IX_LichSans_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LICHSAN",
                newName: "IX_LICHSAN_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon");

            migrationBuilder.RenameIndex(
                name: "IX_HinhAnhs_maSanBong",
                table: "HINHANH",
                newName: "IX_HINHANH_maSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_DonDatSans_MaNhanVien",
                table: "DONDATSAN",
                newName: "IX_DONDATSAN_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_DonDatSans_MaKhachHang",
                table: "DONDATSAN",
                newName: "IX_DONDATSAN_MaKhachHang");

            migrationBuilder.RenameIndex(
                name: "IX_DanhGias_MaSanBong",
                table: "DANHGIA",
                newName: "IX_DANHGIA_MaSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_DanhGias_MaNguoiDung",
                table: "DANHGIA",
                newName: "IX_DANHGIA_MaNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonDatSans_MaSanBong_MaSanCon",
                table: "CHITIETDONDATSAN",
                newName: "IX_CHITIETDONDATSAN_MaSanBong_MaSanCon");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonDatSans_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "CHITIETDONDATSAN",
                newName: "IX_CHITIETDONDATSAN_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "VAITRONGUOIDUNG",
                newName: "IX_VAITRONGUOIDUNG_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SANBONG",
                table: "SANBONG",
                column: "MaSanBong");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NHANVIEN",
                table: "NHANVIEN",
                column: "MaNhanVien");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LICHSAN",
                table: "LICHSAN",
                column: "MaLichSan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HINHANH",
                table: "HINHANH",
                column: "maHinhAnh");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DONDATSAN",
                table: "DONDATSAN",
                column: "MaDatSan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DANHGIA",
                table: "DANHGIA",
                column: "MaDanhGia");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CHITIETSANBONG",
                table: "CHITIETSANBONG",
                columns: new[] { "MaSanBong", "MaSanCon" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CHITIETDONDATSAN",
                table: "CHITIETDONDATSAN",
                columns: new[] { "MaDatSan", "MaSanCon", "MaSanBong", "GioBatDau" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NGUOIDUNG",
                table: "NGUOIDUNG",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VAITRONGUOIDUNG",
                table: "VAITRONGUOIDUNG",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VAITRO",
                table: "VAITRO",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_VAITRO_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "VAITRO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_NGUOIDUNG_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_NGUOIDUNG_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_NGUOIDUNG_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CHITIETDONDATSAN_CHITIETSANBONG_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "CHITIETDONDATSAN",
                columns: new[] { "ChiTietSanBongMaSanBong", "ChiTietSanBongMaSanCon" },
                principalTable: "CHITIETSANBONG",
                principalColumns: new[] { "MaSanBong", "MaSanCon" });

            migrationBuilder.AddForeignKey(
                name: "FK_CHITIETDONDATSAN_CHITIETSANBONG_MaSanBong_MaSanCon",
                table: "CHITIETDONDATSAN",
                columns: new[] { "MaSanBong", "MaSanCon" },
                principalTable: "CHITIETSANBONG",
                principalColumns: new[] { "MaSanBong", "MaSanCon" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CHITIETDONDATSAN_DONDATSAN_MaDatSan",
                table: "CHITIETDONDATSAN",
                column: "MaDatSan",
                principalTable: "DONDATSAN",
                principalColumn: "MaDatSan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CHITIETSANBONG_SANBONG_MaSanBong",
                table: "CHITIETSANBONG",
                column: "MaSanBong",
                principalTable: "SANBONG",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DANHGIA_NGUOIDUNG_MaNguoiDung",
                table: "DANHGIA",
                column: "MaNguoiDung",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DANHGIA_SANBONG_MaSanBong",
                table: "DANHGIA",
                column: "MaSanBong",
                principalTable: "SANBONG",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DONDATSAN_NGUOIDUNG_MaKhachHang",
                table: "DONDATSAN",
                column: "MaKhachHang",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DONDATSAN_NHANVIEN_MaNhanVien",
                table: "DONDATSAN",
                column: "MaNhanVien",
                principalTable: "NHANVIEN",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HINHANH_SANBONG_maSanBong",
                table: "HINHANH",
                column: "maSanBong",
                principalTable: "SANBONG",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LICHSAN_CHITIETSANBONG_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LICHSAN",
                columns: new[] { "ChiTietSanBongMaSanBong", "ChiTietSanBongMaSanCon" },
                principalTable: "CHITIETSANBONG",
                principalColumns: new[] { "MaSanBong", "MaSanCon" });

            migrationBuilder.AddForeignKey(
                name: "FK_LICHSAN_CHITIETSANBONG_MaSanBong_MaSanCon",
                table: "LICHSAN",
                columns: new[] { "MaSanBong", "MaSanCon" },
                principalTable: "CHITIETSANBONG",
                principalColumns: new[] { "MaSanBong", "MaSanCon" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LICHSAN_SANBONG_MaSanBong",
                table: "LICHSAN",
                column: "MaSanBong",
                principalTable: "SANBONG",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LICHSAN_SANBONG_SanBongMaSanBong",
                table: "LICHSAN",
                column: "SanBongMaSanBong",
                principalTable: "SANBONG",
                principalColumn: "MaSanBong");

            migrationBuilder.AddForeignKey(
                name: "FK_NHANVIEN_NGUOIDUNG_MaChuSan",
                table: "NHANVIEN",
                column: "MaChuSan",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_NHANVIEN_NGUOIDUNG_MaNhanVien",
                table: "NHANVIEN",
                column: "MaNhanVien",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_NHANVIEN_SANBONG_MaSanBong",
                table: "NHANVIEN",
                column: "MaSanBong",
                principalTable: "SANBONG",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SANBONG_NGUOIDUNG_MaChuSan",
                table: "SANBONG",
                column: "MaChuSan",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VAITRONGUOIDUNG_NGUOIDUNG_UserId",
                table: "VAITRONGUOIDUNG",
                column: "UserId",
                principalTable: "NGUOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VAITRONGUOIDUNG_VAITRO_RoleId",
                table: "VAITRONGUOIDUNG",
                column: "RoleId",
                principalTable: "VAITRO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_VAITRO_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_NGUOIDUNG_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_NGUOIDUNG_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_NGUOIDUNG_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_CHITIETDONDATSAN_CHITIETSANBONG_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "CHITIETDONDATSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_CHITIETDONDATSAN_CHITIETSANBONG_MaSanBong_MaSanCon",
                table: "CHITIETDONDATSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_CHITIETDONDATSAN_DONDATSAN_MaDatSan",
                table: "CHITIETDONDATSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_CHITIETSANBONG_SANBONG_MaSanBong",
                table: "CHITIETSANBONG");

            migrationBuilder.DropForeignKey(
                name: "FK_DANHGIA_NGUOIDUNG_MaNguoiDung",
                table: "DANHGIA");

            migrationBuilder.DropForeignKey(
                name: "FK_DANHGIA_SANBONG_MaSanBong",
                table: "DANHGIA");

            migrationBuilder.DropForeignKey(
                name: "FK_DONDATSAN_NGUOIDUNG_MaKhachHang",
                table: "DONDATSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_DONDATSAN_NHANVIEN_MaNhanVien",
                table: "DONDATSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_HINHANH_SANBONG_maSanBong",
                table: "HINHANH");

            migrationBuilder.DropForeignKey(
                name: "FK_LICHSAN_CHITIETSANBONG_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LICHSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_LICHSAN_CHITIETSANBONG_MaSanBong_MaSanCon",
                table: "LICHSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_LICHSAN_SANBONG_MaSanBong",
                table: "LICHSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_LICHSAN_SANBONG_SanBongMaSanBong",
                table: "LICHSAN");

            migrationBuilder.DropForeignKey(
                name: "FK_NHANVIEN_NGUOIDUNG_MaChuSan",
                table: "NHANVIEN");

            migrationBuilder.DropForeignKey(
                name: "FK_NHANVIEN_NGUOIDUNG_MaNhanVien",
                table: "NHANVIEN");

            migrationBuilder.DropForeignKey(
                name: "FK_NHANVIEN_SANBONG_MaSanBong",
                table: "NHANVIEN");

            migrationBuilder.DropForeignKey(
                name: "FK_SANBONG_NGUOIDUNG_MaChuSan",
                table: "SANBONG");

            migrationBuilder.DropForeignKey(
                name: "FK_VAITRONGUOIDUNG_NGUOIDUNG_UserId",
                table: "VAITRONGUOIDUNG");

            migrationBuilder.DropForeignKey(
                name: "FK_VAITRONGUOIDUNG_VAITRO_RoleId",
                table: "VAITRONGUOIDUNG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VAITRONGUOIDUNG",
                table: "VAITRONGUOIDUNG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VAITRO",
                table: "VAITRO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SANBONG",
                table: "SANBONG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NHANVIEN",
                table: "NHANVIEN");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NGUOIDUNG",
                table: "NGUOIDUNG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LICHSAN",
                table: "LICHSAN");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HINHANH",
                table: "HINHANH");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DONDATSAN",
                table: "DONDATSAN");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DANHGIA",
                table: "DANHGIA");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CHITIETSANBONG",
                table: "CHITIETSANBONG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CHITIETDONDATSAN",
                table: "CHITIETDONDATSAN");

            migrationBuilder.RenameTable(
                name: "VAITRONGUOIDUNG",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "VAITRO",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "SANBONG",
                newName: "SanBongs");

            migrationBuilder.RenameTable(
                name: "NHANVIEN",
                newName: "NhanViens");

            migrationBuilder.RenameTable(
                name: "NGUOIDUNG",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "LICHSAN",
                newName: "LichSans");

            migrationBuilder.RenameTable(
                name: "HINHANH",
                newName: "HinhAnhs");

            migrationBuilder.RenameTable(
                name: "DONDATSAN",
                newName: "DonDatSans");

            migrationBuilder.RenameTable(
                name: "DANHGIA",
                newName: "DanhGias");

            migrationBuilder.RenameTable(
                name: "CHITIETSANBONG",
                newName: "chiTietSanBongs");

            migrationBuilder.RenameTable(
                name: "CHITIETDONDATSAN",
                newName: "ChiTietDonDatSans");

            migrationBuilder.RenameIndex(
                name: "IX_VAITRONGUOIDUNG_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_SANBONG_MaChuSan",
                table: "SanBongs",
                newName: "IX_SanBongs_MaChuSan");

            migrationBuilder.RenameIndex(
                name: "IX_NHANVIEN_MaSanBong",
                table: "NhanViens",
                newName: "IX_NhanViens_MaSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_NHANVIEN_MaChuSan",
                table: "NhanViens",
                newName: "IX_NhanViens_MaChuSan");

            migrationBuilder.RenameIndex(
                name: "IX_LICHSAN_SanBongMaSanBong",
                table: "LichSans",
                newName: "IX_LichSans_SanBongMaSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_LICHSAN_MaSanBong_MaSanCon",
                table: "LichSans",
                newName: "IX_LichSans_MaSanBong_MaSanCon");

            migrationBuilder.RenameIndex(
                name: "IX_LICHSAN_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "LichSans",
                newName: "IX_LichSans_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon");

            migrationBuilder.RenameIndex(
                name: "IX_HINHANH_maSanBong",
                table: "HinhAnhs",
                newName: "IX_HinhAnhs_maSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_DONDATSAN_MaNhanVien",
                table: "DonDatSans",
                newName: "IX_DonDatSans_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_DONDATSAN_MaKhachHang",
                table: "DonDatSans",
                newName: "IX_DonDatSans_MaKhachHang");

            migrationBuilder.RenameIndex(
                name: "IX_DANHGIA_MaSanBong",
                table: "DanhGias",
                newName: "IX_DanhGias_MaSanBong");

            migrationBuilder.RenameIndex(
                name: "IX_DANHGIA_MaNguoiDung",
                table: "DanhGias",
                newName: "IX_DanhGias_MaNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_CHITIETDONDATSAN_MaSanBong_MaSanCon",
                table: "ChiTietDonDatSans",
                newName: "IX_ChiTietDonDatSans_MaSanBong_MaSanCon");

            migrationBuilder.RenameIndex(
                name: "IX_CHITIETDONDATSAN_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "ChiTietDonDatSans",
                newName: "IX_ChiTietDonDatSans_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanBongs",
                table: "SanBongs",
                column: "MaSanBong");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhanViens",
                table: "NhanViens",
                column: "MaNhanVien");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LichSans",
                table: "LichSans",
                column: "MaLichSan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HinhAnhs",
                table: "HinhAnhs",
                column: "maHinhAnh");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonDatSans",
                table: "DonDatSans",
                column: "MaDatSan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DanhGias",
                table: "DanhGias",
                column: "MaDanhGia");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chiTietSanBongs",
                table: "chiTietSanBongs",
                columns: new[] { "MaSanBong", "MaSanCon" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDonDatSans",
                table: "ChiTietDonDatSans",
                columns: new[] { "MaDatSan", "MaSanCon", "MaSanBong", "GioBatDau" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonDatSans_DonDatSans_MaDatSan",
                table: "ChiTietDonDatSans",
                column: "MaDatSan",
                principalTable: "DonDatSans",
                principalColumn: "MaDatSan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonDatSans_chiTietSanBongs_ChiTietSanBongMaSanBong_ChiTietSanBongMaSanCon",
                table: "ChiTietDonDatSans",
                columns: new[] { "ChiTietSanBongMaSanBong", "ChiTietSanBongMaSanCon" },
                principalTable: "chiTietSanBongs",
                principalColumns: new[] { "MaSanBong", "MaSanCon" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonDatSans_chiTietSanBongs_MaSanBong_MaSanCon",
                table: "ChiTietDonDatSans",
                columns: new[] { "MaSanBong", "MaSanCon" },
                principalTable: "chiTietSanBongs",
                principalColumns: new[] { "MaSanBong", "MaSanCon" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chiTietSanBongs_SanBongs_MaSanBong",
                table: "chiTietSanBongs",
                column: "MaSanBong",
                principalTable: "SanBongs",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_AspNetUsers_MaNguoiDung",
                table: "DanhGias",
                column: "MaNguoiDung",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_SanBongs_MaSanBong",
                table: "DanhGias",
                column: "MaSanBong",
                principalTable: "SanBongs",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonDatSans_AspNetUsers_MaKhachHang",
                table: "DonDatSans",
                column: "MaKhachHang",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonDatSans_NhanViens_MaNhanVien",
                table: "DonDatSans",
                column: "MaNhanVien",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnhs_SanBongs_maSanBong",
                table: "HinhAnhs",
                column: "maSanBong",
                principalTable: "SanBongs",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LichSans_SanBongs_MaSanBong",
                table: "LichSans",
                column: "MaSanBong",
                principalTable: "SanBongs",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_LichSans_chiTietSanBongs_MaSanBong_MaSanCon",
                table: "LichSans",
                columns: new[] { "MaSanBong", "MaSanCon" },
                principalTable: "chiTietSanBongs",
                principalColumns: new[] { "MaSanBong", "MaSanCon" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_AspNetUsers_MaChuSan",
                table: "NhanViens",
                column: "MaChuSan",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_AspNetUsers_MaNhanVien",
                table: "NhanViens",
                column: "MaNhanVien",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_SanBongs_MaSanBong",
                table: "NhanViens",
                column: "MaSanBong",
                principalTable: "SanBongs",
                principalColumn: "MaSanBong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanBongs_AspNetUsers_MaChuSan",
                table: "SanBongs",
                column: "MaChuSan",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
