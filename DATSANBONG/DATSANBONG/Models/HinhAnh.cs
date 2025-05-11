using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATSANBONG.Models
{
    public class HinhAnh
    {
        [Key]
        [StringLength(10)]
        public string maHinhAnh { get; set; }

        [Required]
        [StringLength(10)]
        public string maSanBong { get; set; }

        public string urlHinhAnh { get; set; }

        [ForeignKey("maSanBong")]
        public SanBong SanBong { get; set; }
    }
}
