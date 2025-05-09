using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATSANBONG.Models
{
    public class ChiTietSanBong
    {
        [StringLength(10)]
        public string MaSanBong { get; set; }
        [ForeignKey("MaSanBong")]
        public SanBong SanBong { get; set; }

        [StringLength(10)]
        public string MaSanCon {  get; set; }

        [Required]
        [StringLength(25)]
        public string TenSanCon { get; set; }

        [Required]
        [StringLength(15)]
        public string LoaiSanCon { get; set; }

        [Required]
        [StringLength(15)]
        public string TrangThaiSan {  get; set; }

        public virtual ICollection<LichSan> LichSans { get; set; }

        public virtual ICollection<ChiTietDonDatSan> ChiTietDonDatSans { get; set; }
    }
}
