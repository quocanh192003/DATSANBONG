using System.Net;
using AutoMapper;
using Azure.Core;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATSANBONG.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private APIResponse apiResponse;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            this.apiResponse = new APIResponse();
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Khách hàng đặt sân bóng
        public async Task<APIResponse> CreateOrder(RequestOrderDTO request)
        {
            if (request == null)
            {
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Invalid Data");
                return apiResponse;
            }

            try
            {
                var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

                if (userCurrent == null)
                {
                    apiResponse.Status = HttpStatusCode.Unauthorized;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("Unauthorized");
                    return apiResponse;
                }

                // Lấy id nhân viên của sân bóng để gán vào đơn đặt sân
                var idNhanvien = await _db.NhanViens
                    .Where(x => x.MaSanBong == request.DSSanCon[0].MaSanBong)
                    .Select(x => x.MaNhanVien)
                    .ToListAsync();

                if (idNhanvien == null || idNhanvien.Count == 0)
                {
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("No employee found for this field");
                    return apiResponse;
                }

                var nhanvienDonCount = await _db.DonDatSans
                    .Where(x => x.MaNhanVien != null && idNhanvien.Contains(x.MaNhanVien))
                    .GroupBy(x => x.MaNhanVien)
                    .Select(g => new
                    {
                        MaNhanVien = g.Key,
                        Count = g.Count()
                    }).ToListAsync();

                var nhanVienVoiSoDon = idNhanvien
                    .Select(id => new
                    {
                        MaNhanVien = id,
                        SoDon = nhanvienDonCount.FirstOrDefault(x => x.MaNhanVien == id)?.Count ?? 0
                    }).ToList();
                var nhanVienMin = nhanVienVoiSoDon.OrderBy(x => x.SoDon).FirstOrDefault();

                var count = await _db.DonDatSans.CountAsync();
                var maDatSan = "DS" + (count + 1).ToString("D7");
                var soLuongSan = request.DSSanCon.Count;
                decimal tongTien = 0;

                if (soLuongSan == 0)
                {
                    apiResponse.Status = HttpStatusCode.BadRequest;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("No available fields");
                    return apiResponse;
                }

                var chiTietDatSan = new List<ChiTietDonDatSan>();
                using var transaction = await _db.Database.BeginTransactionAsync();
                foreach (var item in request.DSSanCon)
                {
                    var lichSan = await _db.LichSans
                        .FirstOrDefaultAsync(ls =>
                            ls.MaSanCon == item.MaSanCon &&
                            ls.MaSanBong == item.MaSanBong &&
                            ls.thu == item.thu &&
                            ls.GioBatDau == item.GioBatDau &&
                            ls.GioKetThuc == item.GioKetThuc);

                    if (lichSan == null || lichSan.TrangThai != "AVAILABLE")
                    {
                        apiResponse.Status = HttpStatusCode.BadRequest;
                        apiResponse.IsSuccess = false;
                        apiResponse.ErrorMessages.Add($"Sân {item.MaSanCon} không khả dụng lúc {item.GioBatDau} - {item.GioKetThuc}");
                        return apiResponse;
                    }

                    tongTien += lichSan.GiaThue;

                    // cập nhật trạng thái sân
                    lichSan.TrangThai = "BOOKED";

                    chiTietDatSan.Add(new ChiTietDonDatSan
                    {
                        MaDatSan = maDatSan,
                        MaSanBong = item.MaSanBong,
                        MaSanCon = item.MaSanCon,
                        thu = item.thu,
                        GioBatDau = item.GioBatDau,
                        GioKetThuc = item.GioKetThuc
                    });
                }
                var donDatSan = new DonDatSan
                {
                    MaDatSan = maDatSan,
                    MaKhachHang = userCurrent.Id,
                    MaNhanVien = nhanVienMin.MaNhanVien,
                    NgayDat = DateTime.Now,
                    TrangThai = "PENDING",
                    SoLuongSan = soLuongSan,
                    TongTien = tongTien,
                    PhuongThucTT = request.phuongThucTT,
                    TrangThaiTT = "waiting for payment",
                    ChiTietDonDatSans = chiTietDatSan
                };
                await _db.DonDatSans.AddAsync(donDatSan);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                //var result = await _db.DonDatSans
                //    .Include(d => d.ChiTietDonDatSans)
                //    .ThenInclude(c => c.ChiTietSanBong)
                //    .FirstOrDefaultAsync(d => d.MaDatSan == donDatSan.MaDatSan);
                //if (result == null)
                //{
                //    apiResponse.Status = HttpStatusCode.NotFound;
                //    apiResponse.IsSuccess = false;
                //    apiResponse.ErrorMessages.Add("Order not found");
                //    return apiResponse;
                //}
                //var response = _mapper.Map<ResponseOrderDTO>(result);
                //response.chiTietDonDatSans = result.ChiTietDonDatSans
                //    .Select(c => _mapper.Map<ChiTietDonDatSanDTO>(c))
                //    .ToList();
                apiResponse.Status = HttpStatusCode.Created;
                apiResponse.IsSuccess = true;
                apiResponse.Result = "Bạn đã đặt sân thành công!";
                return apiResponse;

            }
            catch (Exception ex)
            {
                apiResponse.Status = HttpStatusCode.InternalServerError;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add(ex.Message);
                return apiResponse;
            }
        }

        // Lấy ra danh sách đơn đặt sân theo trạng thái
        public async Task<APIResponse> GetAllOrderbyStatus(string status)
        {
            var orders = await _db.DonDatSans
                .Include(x => x.ChiTietDonDatSans)
                .Where(x => x.TrangThai.ToUpper() == status.ToUpper())
                .ToListAsync();
            if (orders == null || orders.Count == 0)
            {
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add($"No {status.ToUpper()} orders found");
                return apiResponse;
            }

            var response = _mapper.Map<List<ResponseOrderDTO>>(orders);
            //foreach (var order in response)
            //{
            //    var chiTietDonDatSans = await _db.ChiTietDonDatSans
            //        .Include(x => x.ChiTietSanBong)
            //        .Where(x => x.MaDatSan == order.maDatSan)
            //        .ToListAsync();
            //    order.chiTietDonDatSans = _mapper.Map<List<ChiTietDonDatSanDTO>>(chiTietDonDatSans);
            //}
            apiResponse.Status = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            apiResponse.Result = response;
            return apiResponse;
        }

        // Lấy ra danh sách đơn đặt sân theo trạng thái thanh toán
        public async Task<APIResponse> GetAllOrderbyStatusTT(string status)
        {
            var orders = await _db.DonDatSans
                .Include(x => x.ChiTietDonDatSans)
                .Where(x => x.TrangThaiTT.ToUpper() == status.ToUpper())
                .ToListAsync();
            if (orders == null || orders.Count == 0)
            {
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add($"No {status.ToUpper()} orders found");
                return apiResponse;
            }

            var response = _mapper.Map<List<ResponseOrderDTO>>(orders);
            //foreach (var order in response)
            //{
            //    var chiTietDonDatSans = await _db.ChiTietDonDatSans
            //        .Include(x => x.ChiTietSanBong)
            //        .Where(x => x.MaDatSan == order.maDatSan)
            //        .ToListAsync();
            //    order.chiTietDonDatSans = _mapper.Map<List<ChiTietDonDatSanDTO>>(chiTietDonDatSans);
            //}
            apiResponse.Status = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            apiResponse.Result = response;
            return apiResponse;
        }

        // Nhân viên xác nhân thanh toán
        public async Task<APIResponse> ConfirmPayment(string idOrder)
        {
            if (string.IsNullOrEmpty(idOrder))
            {
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Invalid Data");
                return apiResponse;
            }
            try
            {
                var donDatSan = await _db.DonDatSans
                     .FirstOrDefaultAsync(x => x.MaDatSan == idOrder);
                if (donDatSan == null)
                {
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("Order not found");
                    return apiResponse;
                }

                if (donDatSan.TrangThaiTT.ToLower() == "paid")
                {
                    apiResponse.Status = HttpStatusCode.OK;
                    apiResponse.IsSuccess = true;
                    apiResponse.Result = "This order has been paid.";
                    return apiResponse;
                }

                donDatSan.TrangThaiTT = "Paid";
                _db.DonDatSans.Update(donDatSan);
                await _db.SaveChangesAsync();
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.Result = "Payment confirmed successfully!";
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.Status = HttpStatusCode.InternalServerError;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add(ex.Message);
                return apiResponse;
            }
        }

        // Nhân viên xác nhận đơn đặt sân + Xác nhận hủy sân + Khách hàng hủy sân
        public async Task<APIResponse> ConfirmOrder(string idOrder, [FromBody] ConfirmOrderStatusDTO status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idOrder) || status == null)
                {
                    apiResponse.Status = HttpStatusCode.BadRequest;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("Invalid Data");
                    return apiResponse;
                }
                var validStatuses = new[] { "CONFIRM", "CONFIRM CANCEL", "CANCEL"};
                if (!validStatuses.Contains(status.status.ToUpper()))
                {
                    apiResponse.Status = HttpStatusCode.BadRequest;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("Invalid status value.");
                    return apiResponse;
                }
                var donDatSan = await _db.DonDatSans
                     .FirstOrDefaultAsync(x => x.MaDatSan == idOrder);
                if (donDatSan == null)
                {
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("Order not found");
                    return apiResponse;
                }


                switch (status.status.ToUpper())
                {
                    case "CONFIRM":
                        donDatSan.TrangThai = "CONFIRMED";
                        break;
                    case "CONFIRM CANCEL":
                        donDatSan.TrangThai = "CANCELED";
                        break;
                    case "CANCEL":
                        donDatSan.TrangThai = "PENDING CANCEL";
                        break;
                }
                //donDatSan.TrangThai = status.status.ToUpper();
                _db.DonDatSans.Update(donDatSan);

                if (status.status.ToUpper() == "CONFIRM CANCEL")
                {
                    var chiTietDatSans = await _db.ChiTietDonDatSans
                        .Where(x => x.MaDatSan == idOrder)
                        .ToListAsync();
                    foreach (var item in chiTietDatSans)
                    {
                        //var lichSan = await _db.LichSans
                        //    .FirstOrDefaultAsync(x => x.MaSanCon == item.MaSanCon && x.MaSanBong == item.MaSanBong);
                        var lichSan = await _db.LichSans
                        .FirstOrDefaultAsync(ls =>
                            ls.MaSanCon == item.MaSanCon &&
                            ls.MaSanBong == item.MaSanBong &&
                            ls.thu == item.thu &&
                            ls.GioBatDau == item.GioBatDau &&
                            ls.GioKetThuc == item.GioKetThuc);
                        if (lichSan != null)
                        {
                            lichSan.TrangThai = "AVAILABLE";
                            _db.LichSans.Update(lichSan);
                        }
                    }

                }
                await _db.SaveChangesAsync();
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                if (status.status.ToUpper() == "CONFIRM")
                    apiResponse.Result = "Order confirmed successfully!";

                if (status.status.ToUpper() == "CONFIRM CANCEL")
                    apiResponse.Result = "Order cancelled successfully!";

                if (status.status.ToUpper() == "CANCEL")
                    apiResponse.Result = "Pending confirm cancel";
                

                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.Status = HttpStatusCode.InternalServerError;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add(ex.Message);
                return apiResponse;
            }
        }

        public async Task<APIResponse> GetOrderByIdUser()
        {
            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (userCurrent == null)
            {
                apiResponse.Status = HttpStatusCode.Unauthorized;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Unauthorized");
                return apiResponse;
            }
            var orders = await _db.DonDatSans
                .Include(x => x.ChiTietDonDatSans)
                .Where(x => x.MaKhachHang == userCurrent.Id)
                .ToListAsync();
            if (orders == null || orders.Count == 0)
            {
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("No orders found");
                return apiResponse;
            }
            var response = _mapper.Map<List<ResponseOrderDTO>>(orders);

            apiResponse.Status = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            apiResponse.Result = response;
            return apiResponse;
        }

        // get order by id staff
        public async Task<APIResponse> GetOrderByIdStaff()
        {
            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (userCurrent == null)
            {
                apiResponse.Status = HttpStatusCode.Unauthorized;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Unauthorized");
                return apiResponse;
            }
            var staffOrders = await _db.DonDatSans
                .Include(x => x.ChiTietDonDatSans)
                .Where(x => x.MaNhanVien == userCurrent.Id)
                .ToListAsync();
            if (staffOrders == null || staffOrders.Count == 0)
            {
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("No orders found for this staff");
                return apiResponse;
            }
            var response = _mapper.Map<List<ResponseOrderDTO>>(staffOrders);
            apiResponse.Status = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            apiResponse.Result = response;
            return apiResponse;
        }
        
    }
}