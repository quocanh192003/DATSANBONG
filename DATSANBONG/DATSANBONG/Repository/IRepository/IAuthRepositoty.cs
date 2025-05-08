using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IAuthRepositoty
    {
        bool IsUniqueUser(string TaiKhoan);
        Task<LoginResponseDTO> Login (LoginRequestDTO model);
        Task<NguoiDungDTO> Register (RegisterRequestDTO model);
        Task<ResponseTokenPasswordDTO> ForgotPassword(RequestForgotPasswordDTO request);
    }
}
