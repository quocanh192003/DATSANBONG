using System.Net;
using AutoMapper;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DATSANBONG.Repository
{
    public class EvaluateRepository : IEvaluateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EvaluateRepository(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _response = new APIResponse();
        }

        public async Task<APIResponse> AverageEvaluate(string masSanBong)
        {
            if (masSanBong == null)
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Tham số không hợp lệ");
                return _response;
            }
            var sanbong = await _context.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == masSanBong);
            if (sanbong == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Sân bóng không tồn tại!");
                return _response;
            }
            var danhgia = await _context.DanhGias
                .Where(x => x.MaSanBong == masSanBong)
                .GroupBy(x => x.MaSanBong)
                .Select(g => new DanhGiaThongKeDTO
                {
                    MaSanBong = g.Key,
                    SoLuongDanhGia = g.Count(),
                    TrungBinhSoSao = g.Average(x => (double?)x.SoSao) ?? 0.0
                })
                .FirstOrDefaultAsync();
            if (danhgia == null)
            {
                var result = new DanhGiaThongKeDTO
                {
                    MaSanBong = masSanBong,
                    SoLuongDanhGia = 0,
                    TrungBinhSoSao = 0.0
                };
                _response.Status = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = result;
                return _response;

            }
            _response.Status = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = danhgia;
            return _response;
        }

        public async Task<APIResponse> createEvaluate(string maSanBong, ResquestCreateEvaluateDTO request)
        {
            if(request == null)
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Tham số không hợp lệ");
                return _response;
            }

            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (string.IsNullOrEmpty(userCurrent.Id))
            {
                _response.Status = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Bạn hãy đăng nhập!");
                return _response;
            }

            var sanbong = await _context.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == maSanBong);
            if (sanbong == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Sân bóng không tồn tại!");
                return _response;
            }

            var maDanhGia = Guid.NewGuid().ToString();
            var evaluate = new DanhGia()
            {
                MaDanhGia = maDanhGia,
                MaSanBong = maSanBong,
                MaNguoiDung = userCurrent.Id,
                SoSao = request.soSao,
                BinhLuan = request.binhLuan,
                NgayDanhGia = DateTime.Now
            };

            await _context.AddAsync(evaluate);
            await _context.SaveChangesAsync();

            _response.Status = HttpStatusCode.Created;
            _response.IsSuccess = true;
            _response.Result = _mapper.Map<ResponseEvaluateDTO>(evaluate);

            return _response;
        }

        public async Task<APIResponse> deleteEvaluate(string maDanhGia)
        {
            if (string.IsNullOrEmpty(maDanhGia))
            {
                _response.IsSuccess= false;
                _response.Status= HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Tham số không hợp lệ");
                return _response;
            }

            var evaluate = await _context.DanhGias.FirstOrDefaultAsync(x => x.MaDanhGia == maDanhGia);
            if (evaluate == null)
            {
                _response.IsSuccess= false;
                _response.Status= HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Đánh giá không được tìm thấy!");
                return _response;

            }
            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (string.IsNullOrEmpty(userCurrent.Id))
            {
                _response.Status = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Bạn hãy đăng nhập!");
                return _response;
            }
            if (evaluate.MaNguoiDung != userCurrent.Id)
            {
                _response.IsSuccess = false;
                _response.Status = HttpStatusCode.Forbidden;
                _response.ErrorMessages.Add("Bạn không có quyền xóa đánh giá này!");
                return _response;
            }

            try
            {

                _context.DanhGias.Remove(evaluate);
                await _context.SaveChangesAsync();

                _response.Status = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Lỗi khi xóa đánh giá: " + ex.Message);
            }

            return _response;
        }

        public async Task<APIResponse> getEvaluateByIdSanBong(string maSanBong)
        {
            if (string.IsNullOrEmpty(maSanBong))
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Tham số không hợp lệ");
                return _response;
            }

            var sanbong = await _context.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == maSanBong);
            if (sanbong == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Sân bóng không tồn tại!");
                return _response;
            }

            var evaluate = await _context.DanhGias
                .Where(x => x.MaSanBong == maSanBong)
                .Include(x => x.NguoiDung)
                .ToListAsync();

            var average = await AverageEvaluate(maSanBong);
            var thongKe = average.Result as DanhGiaThongKeDTO;
            if(evaluate.Count == 0)
            {
                _response.Status = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = new List<ResponseDetailEvaluateDTO>();
                return _response;
            }


            var result = _mapper.Map<List<ResponseDetailEvaluateDTO>>(evaluate);
            foreach (var item in result)
            {
                item.DanhGiaThongKe = thongKe;
            }

            _response.Status = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = result;
            return _response;
        }

        //public Task<APIResponse> getEvaluateByIdUser(string maNguoiDung)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<APIResponse> updateEvaluate(string maDanhGia, UpdateEvaluateDTO request)
        {
            if(string.IsNullOrEmpty(maDanhGia) || request == null)
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Tham số không hợp lệ");
                return _response;
            }
            var evaluate = _context.DanhGias.FirstOrDefault(x => x.MaDanhGia == maDanhGia);
            if (evaluate == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Đánh giá không tồn tại!");
                return _response;
            }
            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (string.IsNullOrEmpty(userCurrent.Id))
            {
                _response.Status = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Bạn hãy đăng nhập!");
                return _response;
            }
            if (evaluate.MaNguoiDung != userCurrent.Id)
            {
                _response.IsSuccess = false;
                _response.Status = HttpStatusCode.Forbidden;
                _response.ErrorMessages.Add("Bạn không có quyền sửa đánh giá này!");
                return _response;
            }
            try
            {
                evaluate.SoSao = request.soSao ?? evaluate.SoSao;
                evaluate.BinhLuan = request.binhLuan ?? evaluate.BinhLuan;
                evaluate.NgayDanhGia = DateTime.Now;

                _context.DanhGias.Update(evaluate);
                await _context.SaveChangesAsync();

                var avgResponse = await AverageEvaluate(evaluate.MaSanBong);
                if (!avgResponse.IsSuccess || avgResponse.Result is not DanhGiaThongKeDTO thongKe)
                {
                    _response.Status = HttpStatusCode.InternalServerError;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không thể tính số sao trung bình.");
                    return _response;
                }

                //var sanBong = await _context.SanBongs.FirstOrDefaultAsync(s => s.MaSanBong == thongKe.MaSanBong);
                //if (sanBong != null)
                //{
                //    sanBong.SoSaoTrungBinh = Math.Round(thongKe.TrungBinhSoSao, 1);
                //    _context.SanBongs.Update(sanBong);
                //    await _context.SaveChangesAsync();
                //}

                // Chuẩn bị dữ liệu trả về
                var responseDTO = new EvaluateUpdatedResponseDTO
                {
                    MaDanhGia = evaluate.MaDanhGia,
                    SoSao = evaluate.SoSao,
                    BinhLuan = evaluate.BinhLuan,
                    NgayCapNhat = evaluate.NgayDanhGia,
                    DanhGiaThongKe = thongKe
                };

                _response.Status = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = responseDTO;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Lỗi khi cập nhật đánh giá: " + ex.Message);
            }

            return _response;
        }
          
    }
}
