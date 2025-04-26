using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IManageUserRepository
    {
        Task<ApplicationUser> ConfirmUser  (ConfirmUserDTO request);
        Task<ApplicationUser> LockUser (ConfirmUserDTO requesr);
    }
}
