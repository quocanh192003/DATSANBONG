using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IFootballFieldRepository
    {
        Task<APIResponse> CreateFootBall(CreateFootballDTO request);
        Task<APIResponse> UpdateFootball(string maSanBong, UpdateFootballDTO request);
        Task<APIResponse> CreateDetailFootball (CreateDetailFootballDTO request);
        Task<APIResponse> UpdateDetailFootball (string masancon, UpdateDetailFootballDTO request);
    }
}
