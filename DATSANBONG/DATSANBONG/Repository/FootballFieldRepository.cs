using AutoMapper;
using Azure;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using DATSANBONG.Services;
using DATSANBONG.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace DATSANBONG.Repository
{
    public class FootballFieldRepository : IFootballFieldRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly APIResponse apiResponse;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public FootballFieldRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
             IHttpContextAccessor httpContextAccessor, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _db = db;
            this.apiResponse = new APIResponse();
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }



        // Chủ sân đăng ký thông tin sân bóng
        public async Task<APIResponse> CreateFootBall(CreateFootballDTO request)
        {
            if (request == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
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
            if (userCurrent == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.Unauthorized;
                apiResponse.ErrorMessages = new List<string>() { "Unauthorized!" };
                return apiResponse;
            }

            using var transaction = await _db.Database.BeginTransactionAsync(); // bắt đầu transaction

            try
            {
                var football = new SanBong()
                {
                    MaSanBong = request.maSanBong,
                    TenSanBong = request.tenSanBong,
                    SoLuongSan = request.soLuongSan,
                    DiaChi = request.diaChi,
                    SoDienThoai = request.soDienThoai,
                    MoTa = request.moTa,
                    //HinhAnh = request.hinhAnh,
                    TrangThai = "PENDING",
                    NgayDangKy = DateTime.Now,
                    MaChuSan = userCurrent.Id,
                };

                await _db.SanBongs.AddAsync(football);
                await _db.SaveChangesAsync();

                // Upload hình ảnh lên Cloudinary
                if (request.HinhAnhFiles != null && request.HinhAnhFiles.Count > 0)
                {
                    foreach (var file in request.HinhAnhFiles)
                    {
                        var imageUrl = await _cloudinaryService.UploadImageAsync(file);

                        if (imageUrl == null)
                        {
                            throw new Exception("Image upload failed!");
                        }

                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            var image = new HinhAnh
                            {
                                maHinhAnh = Guid.NewGuid().ToString("N").Substring(0, 10),
                                maSanBong = football.MaSanBong,
                                urlHinhAnh = imageUrl
                            };
                            await _db.HinhAnhs.AddAsync(image);
                        }
                    }
                    await _db.SaveChangesAsync();

                }

                await transaction.CommitAsync();

                var result = _mapper.Map<ResponseSanBongDTO>(football);

                apiResponse.IsSuccess = true;
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.Result = result;
                return apiResponse;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // rollback khi lỗi
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages = new List<string>() { ex.Message };
            }
            

            return apiResponse;
        }

        // chủ sân cập nhật thông tin sân bóng
        public async Task<APIResponse> UpdateFootball(string maSanBong, UpdateFootballDTO request)
        {
            if (string.IsNullOrWhiteSpace(maSanBong) || request == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
                return apiResponse;
            }

            var football = await _db.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == maSanBong);
            if (football == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string>() { "Football field not found!" };
                return apiResponse;
            }

            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (userCurrent == null || football.MaChuSan != userCurrent.Id)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.Forbidden;
                apiResponse.ErrorMessages = new List<string>() { "You are not authorized to update this field." };
                return apiResponse;
            }
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // Chỉ cập nhật nếu có giá trị mới
                if (!string.IsNullOrWhiteSpace(request.tenSanBong))
                    football.TenSanBong = request.tenSanBong;

                if (request.soLuongSan.HasValue)
                    football.SoLuongSan = request.soLuongSan.Value;

                if (!string.IsNullOrWhiteSpace(request.diaChi))
                    football.DiaChi = request.diaChi;

                if (!string.IsNullOrWhiteSpace(request.soDienThoai))
                    football.SoDienThoai = request.soDienThoai;

                if (!string.IsNullOrWhiteSpace(request.moTa))
                    football.MoTa = request.moTa;

                if (!string.IsNullOrWhiteSpace(request.trangThai))
                    football.TrangThai = request.trangThai;

                // Upload hình ảnh lên Cloudinary
                if (request.hinhAnhFile != null && request.hinhAnhFile.Count > 0)
                {
                    foreach (var file in request.hinhAnhFile)
                    {
                        var imageUrl = await _cloudinaryService.UploadImageAsync(file);

                        if (imageUrl == null)
                        {
                            throw new Exception("Image upload failed!");
                        }

                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            var image = new HinhAnh
                            {
                                maHinhAnh = Guid.NewGuid().ToString("N").Substring(0, 10),
                                maSanBong = football.MaSanBong,
                                urlHinhAnh = imageUrl
                            };
                            await _db.HinhAnhs.AddAsync(image);
                        }
                    }
                    await _db.SaveChangesAsync();

                }

                _db.SanBongs.Update(football);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                var result = _mapper.Map<ResponseSanBongDTO>(football);

                apiResponse.IsSuccess = true;
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.Result = result;
                return apiResponse;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages = new List<string> { ex.Message };
                return apiResponse;
            }
            
        }

        // Chủ sân xóa hình ảnh sân bóng 
        public async Task<APIResponse> DeleteImage(string maHinhAnh)
        {

            try
            {
                var image = await _db.HinhAnhs.FirstOrDefaultAsync(x => x.maHinhAnh == maHinhAnh);

                if (image == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages = new List<string> { "Image not found!" };
                    return apiResponse;
                }

                // Xóa trên Cloudinary
                var result = await _cloudinaryService.DeleteImageAsync(image.urlHinhAnh);
                if (!result)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.BadRequest;
                    apiResponse.ErrorMessages = new List<string> { "Failed to delete image on Cloudinary." };
                    return apiResponse;
                }

                // Xóa trong DB
                _db.HinhAnhs.Remove(image);
                await _db.SaveChangesAsync();

                apiResponse.IsSuccess = true;
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.Result = "Image deleted successfully.";
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages = new List<string> { ex.Message };
                return apiResponse;
            }
        }


        // Chủ sân đăng thông tin chi tiết sân con
        public async Task<APIResponse> CreateDetailFootball(CreateDetailFootballDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.MaSanCon) || string.IsNullOrWhiteSpace(request.MaSanBong))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
                return apiResponse;
            }

            try
            {
                var football = await _db.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == request.MaSanBong);
                if (football == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages = new List<string> { "Parent football field not found!" };
                    return apiResponse;
                }

                var detail = await _db.chiTietSanBongs.FirstOrDefaultAsync(x => x.MaSanCon == request.MaSanCon);
                if (detail != null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.Conflict;
                    apiResponse.ErrorMessages = new List<string> { "Sub football field already exists!" };
                    return apiResponse;
                }

                var sancon = _mapper.Map<ChiTietSanBong>(request);
                sancon.TrangThaiSan = "AVAILABLE";
                await _db.chiTietSanBongs.AddAsync(sancon);
                await _db.SaveChangesAsync();
                var sanconDTO = _mapper.Map<ResponseDetailFootballDTO>(sancon);

                apiResponse.IsSuccess = true;
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.Result = sanconDTO;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return apiResponse;
            }
        }

        // Nhân viên update thông tin sân con (loại sân, trạng thái sân)
        public async Task<APIResponse> UpdateDetailFootball(string masancon, UpdateDetailFootballDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(masancon))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
                return apiResponse;
            }
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Kiểm tra quyền: user có phải là nhân viên của sân chứa sân con này không?
                var isNhanVienSan = await _db.NhanViens
                    .AnyAsync(nv =>
                        nv.MaNhanVien == userId &&
                        _db.chiTietSanBongs
                            .Any(ct => ct.MaSanCon == masancon && ct.MaSanBong == nv.MaSanBong)
                    );
                if (!isNhanVienSan)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.Forbidden;
                    apiResponse.ErrorMessages = new List<string> { "Bạn không có quyền cập nhật chi tiết sân bóng này." };
                    return apiResponse;
                }

                var checkdetail = await _db.chiTietSanBongs.FirstOrDefaultAsync(x => x.MaSanCon == masancon);
                if (checkdetail == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages = new List<string>() { "Detail Football not found!" };
                    return apiResponse;
                }

                if (!string.IsNullOrWhiteSpace(request.TenSanCon))
                {
                    checkdetail.TenSanCon = request.TenSanCon;
                }

                if (!string.IsNullOrWhiteSpace(request.LoaiSanCon))
                {
                    checkdetail.LoaiSanCon = request.LoaiSanCon;
                }

                if (!string.IsNullOrWhiteSpace(request.TrangThaiSan))
                {
                    var status = request.TrangThaiSan.Trim().ToUpper();
                    if (status == "ACTIVE" || status == "INACTIVE")
                    {
                        checkdetail.TrangThaiSan = status;
                    }
                }

                _db.chiTietSanBongs.Update(checkdetail);
                await _db.SaveChangesAsync();

                var result = _mapper.Map<ResponseDetailFootballDTO>(checkdetail);
                apiResponse.IsSuccess = true;
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.Result = result;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return apiResponse;
            }
        }

        // Lấy ra tất cả sân bóng 
        public async Task<APIResponse> GetAllFootballField()
        {
            try
            {
                var footballFields = await _db.SanBongs
                    .Include(sb => sb.HinhAnhs) // Include hình ảnh
                    .ToListAsync();

                if (footballFields == null || !footballFields.Any())
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        Status = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string>() { "No football fields found!" }
                    };
                }

                var footballFieldsDTO = _mapper.Map<List<SanBongDTO>>(footballFields);

                return new APIResponse
                {
                    IsSuccess = true,
                    Status = HttpStatusCode.OK,
                    Result = footballFieldsDTO
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Status = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string>() { ex.Message }
                };
            }
        }

        // Lay ra thông tin sân bóng theo mã sân bóng
        public async Task<APIResponse> GetFootballFieldById(string maSanBong)
        {
            if (string.IsNullOrWhiteSpace(maSanBong))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
                return apiResponse;
            }
            var footballField = await _db.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == maSanBong);
            if (footballField == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string>() { "Football field not found!" };
                return apiResponse;
            }
            var footballFieldDTO = _mapper.Map<SanBongDTO>(footballField);
            apiResponse.IsSuccess = true;
            apiResponse.Status = HttpStatusCode.OK;
            apiResponse.Result = footballFieldDTO;
            return apiResponse;
        }

        // Lấy ra thông tin chi tiết sân bóng theo mã sân con   
        public async Task<APIResponse> GetDetailFootballFieldById(string maSanCon)
        {
            if (string.IsNullOrWhiteSpace(maSanCon))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
                return apiResponse;
            }
            var detailFootballField = await _db.chiTietSanBongs.FirstOrDefaultAsync(x => x.MaSanCon == maSanCon);
            if (detailFootballField == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string>() { "Detail football field not found!" };
                return apiResponse;
            }
            var detailFootballFieldDTO = _mapper.Map<ResponseDetailFootballDTO>(detailFootballField);
            apiResponse.IsSuccess = true;
            apiResponse.Status = HttpStatusCode.OK;
            apiResponse.Result = detailFootballFieldDTO;
            return apiResponse;
        }

        // Lấy ra tất cả chi tiết sân bóng
        public async Task<APIResponse> GetAllDetailFootballField()
        {
            try
            {
                var detailFootballFields = await _db.chiTietSanBongs.ToListAsync();
                if (detailFootballFields == null || !detailFootballFields.Any())
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages = new List<string>() { "No detail football fields found!" };
                    return apiResponse;
                }
                var detailFootballFieldsDTO = _mapper.Map<List<ResponseDetailFootballDTO>>(detailFootballFields);
                apiResponse.IsSuccess = true;
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.Result = detailFootballFieldsDTO;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return apiResponse;
            }
        }

        public async Task<APIResponse> GetDetailFootballbyStatus(string masanbong)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(masanbong))
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.BadRequest;
                    apiResponse.ErrorMessages = new List<string>() { "Invalid Information!" };
                    return apiResponse;
                }

                var checkSanBong = await _db.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == masanbong);
                if (checkSanBong == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages = new List<string>() { "Football field not found!" };
                    return apiResponse;
                }

                var lichsanList = await _db.LichSans
                    .Where(x => x.MaSanBong == masanbong && x.TrangThai == "AVAILABLE")
                    .ToListAsync();

                if (!lichsanList.Any())
                {
                    apiResponse.IsSuccess = true;
                    apiResponse.Status = HttpStatusCode.OK;
                    apiResponse.Result = new List<object>(); // Trống
                    return apiResponse;
                }
                var result = from ls in lichsanList
                             join ct in _db.chiTietSanBongs
                             on new { ls.MaSanCon, ls.MaSanBong } equals new { ct.MaSanCon, ct.MaSanBong }
                             orderby ls.thu, ls.GioBatDau
                             select new
                             {
                                 ct.MaSanCon,
                                 ct.TenSanCon,
                                 ct.LoaiSanCon,
                                 ls.MaLichSan,
                                 ls.thu,
                                 ls.GioBatDau,
                                 ls.GioKetThuc,
                                 ls.GiaThue,
                                 ls.TrangThai
                             };

                if (!result.Any())
                {
                    apiResponse.IsSuccess = true;
                    apiResponse.Status = HttpStatusCode.OK;
                    apiResponse.Result = new List<object>(); // Trống
                    return apiResponse;
                }
                apiResponse.IsSuccess = true;
                apiResponse.Status = HttpStatusCode.OK;
                apiResponse.Result = result;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return apiResponse;
            }
        }
    }
}
