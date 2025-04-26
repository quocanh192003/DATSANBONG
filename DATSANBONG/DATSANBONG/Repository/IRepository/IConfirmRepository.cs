using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IConfirmRepository
    {
        Task<ApplicationUser> ConfirmUser  (ConfirmUserDTO request);
    }
}
