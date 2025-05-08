using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATSANBONG.Models
{
    public class DonDatSan
    {
        [Key]
        [StringLength(10)]
        public string MaDatSan { get; set; }

        [Required]
        [StringLength(450)]
        public string MaKhachHang { get; set; }

        [Required]
        [StringLength(450)]
        public string MaNhanVien { get; set; }

        public DateTime NgayDat { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        public int SoLuongSan { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TongTien { get; set; }

        [StringLength(20)]
        public string PhuongThucTT { get; set; }

        [StringLength(20)]
        public string TrangThaiTT { get; set; }

        // === Quan hệ ===

        [ForeignKey("MaKhachHang")]
        public virtual ApplicationUser KhachHang { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; }

        public virtual ICollection<ChiTietDonDatSan> ChiTietDonDatSans { get; set; }

    }
}
