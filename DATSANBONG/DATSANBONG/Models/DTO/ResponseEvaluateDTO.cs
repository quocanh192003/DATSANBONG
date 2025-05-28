namespace DATSANBONG.Models.DTO
{
    public class ResponseEvaluateDTO
    {
        public string MaDanhGia { get; set; }
        public string MaSanBong { get; set; }
        public int SoSao { get; set; }
        public string BinhLuan { get; set; }
        public DateTime NgayDanhGia { get; set; }
        public NguoiDungEvaluateDTO NguoiDung { get; set; }
    }

}
