using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models
{
    public class ApplicationUser : IdentityUser
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int MaNguoiDung { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string TaiKhoan { get; set; }

        //[Required]
        //[StringLength(20)]
        //public string MatKhau { get; set; }

        [Required]
        [StringLength(50)]
        public string HoTen { get; set; }

        public DateTime? NgaySinh { get; set; }

        [StringLength(10)]
        public string GioiTinh { get; set; }

        [Required]
        [StringLength(25)]
        public string TrangThai { get; set; } = "PENDING";

        //[Required]
        //[EmailAddress]
        //[StringLength(50)]
        //public string EMail { get; set; }

        [Required]
        [StringLength(10)]
        public string MaVaiTro { get; set; }

        //[ForeignKey("MaVaiTro")]
        //public VaiTro VaiTro { get; set; }

        //public ICollection<SanBong> DanhSachSan { get; set; }
        //public ICollection<NhanVien> DanhSachNhanVien { get; set; }
    }
}
