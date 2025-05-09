namespace DATSANBONG.Models.DTO
{
    public class RequestResetPasswordUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
