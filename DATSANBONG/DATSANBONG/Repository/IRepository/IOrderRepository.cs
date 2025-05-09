using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<APIResponse> CreateOrder (RequestOrderDTO request);
    }
}
