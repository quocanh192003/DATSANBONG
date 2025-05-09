using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATSANBONG.Models
{
    public class SanBong
    {
        [Key]
        [StringLength(10)]
        public string MaSanBong { get; set; }

        [Required]
        [StringLength(25)]
        public string TenSanBong { get; set; }

        [Required]
        public int SoLuongSan { get; set; }

        [Required]
        [StringLength(50)]
        public string DiaChi { get; set; }

        [StringLength(10)]
        public string SoDienThoai { get; set; }

        public string MoTa { get; set; } // TEXT nên cứ để string

        [StringLength(15)]
        public string TrangThai { get; set; }

        public DateTime? NgayDangKy { get; set; }

        [Required]
        [StringLength(450)]
        public string MaChuSan { get; set; } // FK tới ApplicationUser.Username

        [ForeignKey("MaChuSan")]
        public ApplicationUser ChuSan { get; set; }

        public string HinhAnh { get; set; }

        // Navigation
        public ICollection<NhanVien> DanhSachNhanVien { get; set; }

        public ICollection<ChiTietSanBong> DanhSachChiTietSan { get; set; }
        public ICollection<LichSan> DanhSachLichSan{ get; set; }


    }
}
