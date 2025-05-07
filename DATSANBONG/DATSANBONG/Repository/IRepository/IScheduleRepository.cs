using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IScheduleRepository
    {
        Task<APIResponse> CreatSchedule(LichSanDTO request);
        Task<APIResponse> GetAllSchedule();
        Task<APIResponse> GetScheduleByIdDetailFootball(string id);
        Task<APIResponse> UpdateSchedule(string id, UpdateScheduleDTO request);
        Task<APIResponse> DeleteSchedule(string id);
    }
}
