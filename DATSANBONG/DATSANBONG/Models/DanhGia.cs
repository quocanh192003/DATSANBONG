using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models
{
    public class DanhGia
    {
        [Key]
        public string MaDanhGia { get; set; }

        [Required]
        public string MaNguoiDung { get; set; }

        [Required]
        public string MaSanBong { get; set; }

        public int SoSao { get; set; }

        public string BinhLuan { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayDanhGia { get; set; }

        // Navigation properties
        [ForeignKey("MaNguoiDung")]
        public virtual ApplicationUser NguoiDung { get; set; }

        [ForeignKey("MaSanBong")]
        public virtual SanBong SanBong { get; set; }
    }
}
