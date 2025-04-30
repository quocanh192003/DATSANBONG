using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IFootballFieldRepository
    {
        Task<APIResponse> CreateFootBall(CreateFootballDTO request);
    }
}
