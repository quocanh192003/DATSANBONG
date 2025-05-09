namespace DATSANBONG.Models.DTO
{
    public class ResponseOrderDTO
    {
        public string maDatSan { get; set; }
        public string maKhachHang { get; set; }
        public string maNhanVien { get; set; }
        public DateTime ngayDat { get; set; }
        public string trangThai { get; set; }
        public string soLuongSan { get; set; }
        public Decimal tongTien { get; set; }
        public string phuongThucTT { get; set; }
        public string trangThaiTT { get; set; }
        public List<ChiTietDonDatSanDTO> chiTietDonDatSans { get; set; }
    }
}
