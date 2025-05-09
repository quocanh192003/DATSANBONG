using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models
{
    public class ChiTietDonDatSan
    {
        [StringLength(10)]
        public string MaDatSan { get; set; }

        [ForeignKey("MaDatSan")]
        public DonDatSan DonDatSan { get; set; }

        [StringLength(10)]
        public string MaSanBong { get; set; }

        [StringLength(10)]
        public string MaSanCon { get; set; }

        [StringLength(10)]
        public string thu { get; set; }

        [Required]
        public TimeSpan GioBatDau { get; set; }

        [Required]
        public TimeSpan GioKetThuc { get; set; }

        [ForeignKey("MaSanBong, MaSanCon")]
        public ChiTietSanBong ChiTietSanBong { get; set; }
    }
}
