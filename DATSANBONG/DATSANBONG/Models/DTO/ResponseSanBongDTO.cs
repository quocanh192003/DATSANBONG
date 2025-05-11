namespace DATSANBONG.Models.DTO
{
    public class ResponseSanBongDTO
    {
        public string MaSanBong { get; set; }
        public string TenSanBong { get; set; }
        public int SoLuongSan { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }

        public List<ResponseHinhAnhDTO> HinhAnh { get; set; }
    }
}
