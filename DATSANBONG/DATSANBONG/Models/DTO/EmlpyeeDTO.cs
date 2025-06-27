using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models.DTO
{
    public class EmlpyeeDTO
    {
        public string MaNhanVien { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        [Required]
        public string TenVaiTro { get; set; }
        public string MaSanBong { get; set; }
    }
}
