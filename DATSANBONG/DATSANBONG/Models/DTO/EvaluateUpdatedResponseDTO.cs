namespace DATSANBONG.Models.DTO
{
    public class EvaluateUpdatedResponseDTO
    {
        public string MaDanhGia { get; set; }
        public int SoSao { get; set; }
        public string BinhLuan { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public DanhGiaThongKeDTO DanhGiaThongKe { get; set; }
    }
}
