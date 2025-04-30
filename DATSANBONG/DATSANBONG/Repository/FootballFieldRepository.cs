using Azure;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DATSANBONG.Repository
{
    public class FootballFieldRepository : IFootballFieldRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly APIResponse apiResponse;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public FootballFieldRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            this.apiResponse = new APIResponse();
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse> CreateFootBall(CreateFootballDTO request)
        {
            if(request == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!"};
                return apiResponse;
            }

            if (string.IsNullOrWhiteSpace(request.maSanBong))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
                return apiResponse;
            }

            var checkID = await _db.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == request.maSanBong);
            if (checkID != null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "The football field already exists!" };
                return apiResponse;
            }

            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            var football = new SanBong()
            {
                MaSanBong = request.maSanBong,
                TenSanBong = request.tenSanBong,
                SoLuongSan = request.soLuongSan,
                DiaChi = request.diaChi,
                SoDienThoai = request.soDienThoai,
                MoTa = request.moTa,
                HinhAnh = request.hinhAnh,
                TrangThai = "PENDING",
                NgayDangKy = DateTime.Now,
                MaChuSan = userCurrent.Id,
            };

            await _db.SanBongs.AddAsync(football);
            await _db.SaveChangesAsync();

            apiResponse.IsSuccess = true;
            apiResponse.Status = HttpStatusCode.OK;
            apiResponse.Result = football;
            return apiResponse;
        }
    }
}
