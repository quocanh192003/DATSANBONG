using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models.DTO
{
    public class LichSanDTO
    {
        public string MaLichSan { get; set; }
        public string MaSanCon { get; set; }
        public string MaSanBong { get; set; }
        public string thu { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public decimal GiaThue { get; set; }
        //public string TrangThai { get; set; }
    }
}
