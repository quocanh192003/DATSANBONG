namespace DATSANBONG.Models.DTO
{
    public class UpdateFootballDTO
    {
        public string? tenSanBong { get; set; }
        public int? soLuongSan { get; set; }
        public string? diaChi { get; set; }
        public string? soDienThoai { get; set; }
        public string? moTa { get; set; }
        public List<IFormFile> hinhAnhFile { get; set; }
        public string? trangThai { get; set; }
    }
}
