using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<APIResponse> CreateOrder (RequestOrderDTO request);
        Task<APIResponse> GetAllOrderbyStatus(string status);
        Task<APIResponse> GetAllOrderbyStatusTT(string status);

        Task<APIResponse> ConfirmPayment(string idOrder);
        Task<APIResponse> ConfirmOrder(string idOrder, ConfirmOrderStatusDTO status);
        Task<APIResponse> GetOrderByIdUser();

    }
}
