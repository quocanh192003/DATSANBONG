using System.ComponentModel.DataAnnotations;

namespace DATSANBONG.Models.DTO
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        [Required]
        public string TenVaiTro { get; set; }
    }
}
