namespace DATSANBONG.Models.DTO
{
    public class LoginResponseDTO
    {
        public NguoiDungDTO User { get; set; }
        public string Role  { get; set; }
        public string Token { get; set; }
    }
}
