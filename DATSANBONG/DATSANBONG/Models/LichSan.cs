using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATSANBONG.Models
{
    public class LichSan
    {
        [Key]
        public string MaLichSan { get; set; }
        [Required]
        public string MaSanCon { get; set; }
        [Required]
        public string MaSanBong { get; set; }
        [Required]
        public string thu { get; set; }
        [Required]
        public TimeSpan GioBatDau { get; set; }
        [Required]
        public TimeSpan GioKetThuc { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal GiaThue { get; set; }
        [Required]
        public string TrangThai { get; set; }

        // Quan hệ đến bảng CHITIETSANBONG (khoá phụ kép)
        [ForeignKey(nameof(MaSanBong) + "," + nameof(MaSanCon))]
        public ChiTietSanBong ChiTietSanBong { get; set; }

        // Quan hệ đến bảng SANBONG (đơn khoá)
        [ForeignKey("MaSanBong")]
        public SanBong SanBong { get; set; }
    }
}
