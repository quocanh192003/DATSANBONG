using DATSANBONG.Models;
using DATSANBONG.Models.DTO;


namespace DATSANBONG.Repository.IRepository
{
    public interface IEvaluateRepository
    {
        Task<APIResponse> createEvaluate(string maSanBong, ResquestCreateEvaluateDTO request);
        Task<APIResponse> updateEvaluate(string maDanhGia, UpdateEvaluateDTO request);
        Task<APIResponse> getEvaluateByIdSanBong(string maSanBong);
        //Task<APIResponse> getEvaluateByIdUser(string maNguoiDung);
        Task<APIResponse> deleteEvaluate(string maDanhGia);
        Task<APIResponse> AverageEvaluate(string masSanBong);

    }
}
