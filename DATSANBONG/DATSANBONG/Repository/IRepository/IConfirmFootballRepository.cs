using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IConfirmFootballRepository
    {
        Task<SanBongDTO> confirmFootball(string Id, string status);
        Task<SanBongDTO> GetFootballById(string Id);
        Task<List<SanBongDTO>> GetAllFootball();
    }
}
