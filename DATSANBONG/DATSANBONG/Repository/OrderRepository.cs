using System.Net;
using AutoMapper;
using Azure.Core;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
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

                var result = await _db.DonDatSans
                    .Include(d => d.ChiTietDonDatSans)
                    .ThenInclude(c => c.ChiTietSanBong)
                    .FirstOrDefaultAsync(d => d.MaDatSan == donDatSan.MaDatSan);
                if (result == null)
                {
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages.Add("Order not found");
                    return apiResponse;
                }
                var response = _mapper.Map<ResponseOrderDTO>(result);
                response.chiTietDonDatSans = result.ChiTietDonDatSans
                    .Select(c => _mapper.Map<ChiTietDonDatSanDTO>(c))
                    .ToList();
                apiResponse.Status = HttpStatusCode.Created;
                apiResponse.IsSuccess = true;
                apiResponse.Result = response;
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
    }

}
