using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models
{
    public class NhanVien
    {
        [Key]
        [StringLength(450)]
        public string MaNhanVien { get; set; }

        [Required]
        [StringLength(450)]
        public string MaChuSan { get; set; } 

        [Required]
        [StringLength(10)]
        public string MaSanBong { get; set; }

        // Foreign Key mappings
        [ForeignKey(nameof(MaNhanVien))]
        public ApplicationUser NhanVienUser { get; set; }

        [ForeignKey(nameof(MaChuSan))]
        public ApplicationUser ChuSan { get; set; }

        [ForeignKey(nameof(MaSanBong))]
        public SanBong SanBong { get; set; }
    }
}
