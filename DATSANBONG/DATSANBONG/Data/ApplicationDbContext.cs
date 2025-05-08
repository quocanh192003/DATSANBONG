using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DATSANBONG.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace DATSANBONG.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<SanBong> SanBongs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<ChiTietSanBong> chiTietSanBongs { get; set; }
        public DbSet<LichSan> LichSans { get; set; }
        public DbSet<DonDatSan> DonDatSans { get; set; }
        public DbSet<ChiTietDonDatSan> ChiTietDonDatSans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key cho ChiTietSanBong
            modelBuilder.Entity<ChiTietSanBong>()
                .HasKey(ct => new { ct.MaSanBong, ct.MaSanCon });

            // Quan hệ 1-n: SanBong -> ChiTietSanBong
            modelBuilder.Entity<ChiTietSanBong>()
                .HasOne(ct => ct.SanBong)
                .WithMany(sb => sb.DanhSachChiTietSan)
                .HasForeignKey(ct => ct.MaSanBong);

            modelBuilder.Entity<LichSan>()
                .HasOne(ls => ls.ChiTietSanBong)
                .WithMany()
                .HasForeignKey(ls => new { ls.MaSanBong, ls.MaSanCon })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichSan>()
                .HasOne(ls => ls.SanBong)
                .WithMany()
                .HasForeignKey(ls => ls.MaSanBong)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietDonDatSan>()
                .HasKey(c => new { c.MaDatSan, c.MaSanCon, c.MaSanBong, c.GioBatDau });

            modelBuilder.Entity<ChiTietDonDatSan>()
                .HasOne(c => c.DonDatSan)
                .WithMany(d => d.ChiTietDonDatSans)
                .HasForeignKey(c => c.MaDatSan);

            modelBuilder.Entity<ChiTietDonDatSan>()
                .HasOne(c => c.ChiTietSanBong)
                .WithMany()
                .HasForeignKey(c => new { c.MaSanBong, c.MaSanCon });
        }
    }
}
