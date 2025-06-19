namespace DATSANBONG.Models.DTO
{
    public class SanBongDTO
    {
        public string maSanBong { get; set; }
        public string tenSanBong { get; set; }
        public int soLuongSan { get; set; }
        public string diaChi { get; set; }
        public string soDienThoai {  get; set; }
        public string moTa {  get; set; }
        public DateTime ngayDangKy { get; set; }
        //public string hinhAnh { get; set; }
        public string trangThai {  get; set; }
        public string maChuSan { get; set; }
        public List<HinhAnhDTO> HinhAnhs { get; set; }
    }
}
