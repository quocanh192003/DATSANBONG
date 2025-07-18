﻿using AutoMapper;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using DATSANBONG.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Mvc;

namespace DATSANBONG.Repository
{
    public class ManageUserRepository : IManageUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly APIResponse response;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ManageUserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            IEmailService emailService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _emailService = emailService;
            this.response = new APIResponse();
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        //ADMIN COMFIRM USER
        public async Task<ApplicationUser> ConfirmUser(string id, string status)
        {
            if (status == null || status.ToUpper() != "ACTIVE")
            {
                return null;
            }

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (user.TrangThai.ToUpper() == "PENDING")
                {
                    user.TrangThai = status.ToUpper();
                    _db.ApplicationUsers.Update(user);
                    await _db.SaveChangesAsync();
                    return user;
                }
                return user;
            }
        }

        // ADMIN UPDATE STATUS USER (ACTIVE -> INACTIVE)
        public async Task<ApplicationUser> LockUser(ConfirmUserDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TrangThai))
            {
                return null;
            }

            var status = request.TrangThai.ToUpper();
            if (status != "INACTIVE")
            {
                return null;
            }

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.ID);
            if (user == null)
            {
                return null;
            }
            else
            {
                if (user.TrangThai.ToUpper() == "ACTIVE")
                {
                    user.TrangThai = status;
                    _db.ApplicationUsers.Update(user);
                    await _db.SaveChangesAsync();
                    return user;
                }
                return null;
            }
        }

        // ADMIN LOCK USER (IDENTITY)
        public async Task<APIResponse> LockUnlockUserAsync(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.NotFound;
                response.ErrorMessages.Add("User not found");
                return response;
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
            {
                user.LockoutEnd = DateTime.UtcNow;
            }
            else
            {
                user.LockoutEnd = DateTime.UtcNow.AddYears(100);
            }
            //user.UpdatedDate = DateTimeOffset.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.BadRequest;
                response.ErrorMessages.Add("Failed to update user lock status");
                return response;
            }

            response.IsSuccess = true;
            response.Status = HttpStatusCode.OK;
            response.Result = "User lock/unlock operation successful";
            return response;
        }

        // CHỦ SÂN CREATE NHAN VIÊN
        public async Task<APIResponse> AddEmployee(EmlpyeeDTO request)
        {
            if (request == null)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.BadRequest;
                response.ErrorMessages = new List<string> { "Yêu cầu không hợp lệ." };
                return response;
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.TenVaiTro) ||
                string.IsNullOrWhiteSpace(request.HoTen) ||
                request.MaSanBong == null)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.BadRequest;
                response.ErrorMessages = new List<string> { "Vui lòng điền đầy đủ các thông tin bắt buộc." };
                return response;
            }

            if (!new EmailAddressAttribute().IsValid(request.Email))
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.BadRequest;
                response.ErrorMessages = new List<string> { "Email không hợp lệ." };
                return response;
            }

            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (currentUser == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.Unauthorized;
                    response.ErrorMessages = new List<string> { "Không thể lấy thông tin tài khoản hiện tại. Vui lòng đăng nhập lại." };
                    return response;
                }

                var employee1 = new ApplicationUser
                {
                    UserName = request.Username,
                    HoTen = request.HoTen,
                    Email = request.Email,
                    NgaySinh = request.NgaySinh,
                    TrangThai = "PENDING",
                    NormalizedEmail = request.Email.ToUpper(),
                    GioiTinh = request.GioiTinh,
                    PhoneNumber = request.SoDienThoai,

                    // ✅ Bỏ xác thực email
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(employee1, request.Password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.ErrorMessages = createResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                var roleResult = await _userManager.AddToRoleAsync(employee1, request.TenVaiTro.Trim());
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.ErrorMessages = roleResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                var employee2 = new NhanVien
                {
                    MaNhanVien = employee1.Id,
                    MaChuSan = currentUser.Id,
                    MaSanBong = request.MaSanBong
                };

                _db.NhanViens.Add(employee2);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                response.IsSuccess = true;
                response.Status = HttpStatusCode.OK;
                response.Result = "Tạo tài khoản nhân viên thành công";
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsSuccess = false;
                response.Status = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }


        // CHỦ SÂN REMOVE NHÂN VIÊN
        public async Task<APIResponse> DeleteEmployee(string EmployeeId)
        {
            if (string.IsNullOrWhiteSpace(EmployeeId))
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.BadRequest;
                response.ErrorMessages = new List<string> { "Invalid User Information" };
                return response;
            }
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Xóa nhân viên ở bảng applicationUser
                var user = await _userManager.FindByIdAsync(EmployeeId);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string> { "User is not found!" };
                    return response;
                }
                // Xóa role của nhân viên trong bảng AspNetRole
                var role = await _userManager.GetRolesAsync(user);
                if (role.Any())
                {
                    var roleRemoveResult = await _userManager.RemoveFromRolesAsync(user, role);
                    if (!roleRemoveResult.Succeeded)
                    {
                        response.IsSuccess = false;
                        response.Status = HttpStatusCode.BadRequest;
                        response.ErrorMessages = new List<string>() { "Employee roles can't be deleted." };
                        return response;
                    }
                }

                // Xóa nhân viên trong bảng NhanVien
                var employee = await _db.NhanViens.FindAsync(EmployeeId);
                if (employee == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.ErrorMessages = new List<string>() { "Employee can't be deleted." };
                    return response;
                }

                _db.NhanViens.Remove(employee);
                await _db.SaveChangesAsync();
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.ErrorMessages = new List<string>()
            {
                "Employee can't be deleted."
            };
                    return response;
                }
                await transaction.CommitAsync();
                response.IsSuccess = true;
                response.Status = HttpStatusCode.OK;
                response.Result = "Delete Employee Successfull";
                return response;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsSuccess = false;
                response.Status = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { "Error: " + ex.Message };
                return response;
            }
        }

        // LẤY RA TẤT CẢ NHÂN VIÊN
        public async Task<APIResponse> GetAllEmployees()
        {
            var response = new APIResponse();
            try
            {
                var employees = await (from nv in _db.NhanViens
                                       join user in _db.Users on nv.MaNhanVien equals user.Id
                                       select new EmlpyeeDTO
                                       {
                                           MaNhanVien = nv.MaNhanVien,
                                           Username = user.UserName,
                                           HoTen = user.HoTen,
                                           Email = user.Email,
                                           GioiTinh = user.GioiTinh,
                                           SoDienThoai = user.PhoneNumber,
                                           MaSanBong = nv.MaSanBong
                                           
                                       }).ToListAsync();

                response.IsSuccess = true;
                response.Status = HttpStatusCode.OK;
                response.Result = employees;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }

        // LẤY NHÂN VIÊN THEO ID
        public async Task<APIResponse> GetEmployee(string id)
        {
            var employee = await (from nv in _db.NhanViens
                                  join user in _db.Users on nv.MaNhanVien equals user.Id
                                  where user.Id == id
                                  select new EmlpyeeDTO
                                  {
                                      MaNhanVien = nv.MaNhanVien,
                                      Username = user.UserName,
                                      HoTen = user.HoTen,
                                      Email = user.Email,

                                      GioiTinh = user.GioiTinh,
                                      SoDienThoai = user.PhoneNumber,
                                      MaSanBong = nv.MaSanBong
                                  }).FirstOrDefaultAsync();

            if (employee == null)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.NotFound;
                response.ErrorMessages = new List<string> { "Không tìm thấy nhân viên." };
                return response;
            }


            response.IsSuccess = true;
            response.Status = HttpStatusCode.OK;
            response.Result = employee;
            return response;

        }

        // UPDATE PROFILE
        public async Task<APIResponse> UpdateInfo(UpdateInfoDTO request)
        {
            try
            {
                var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (userCurrent == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.Unauthorized;
                    response.ErrorMessages = new List<string>() { "Người dùng chưa đăng nhập hoặc phiên đăng nhập đã hết hạn." };
                    return response;
                }

                // Cập nhật từng trường nếu có dữ liệu
                if (!string.IsNullOrWhiteSpace(request.HoTen))
                    userCurrent.HoTen = request.HoTen;

                if (request.NgaySinh.HasValue)
                    userCurrent.NgaySinh = request.NgaySinh.Value;

                if (!string.IsNullOrWhiteSpace(request.GioiTinh))
                    userCurrent.GioiTinh = request.GioiTinh;

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                    userCurrent.PhoneNumber = request.PhoneNumber;

                var result = await _userManager.UpdateAsync(userCurrent);
                if (!result.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.ErrorMessages = result.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                response.IsSuccess = true;
                response.Status = HttpStatusCode.OK;
                response.Result = userCurrent;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }

        // LẤY THÔNG TIN NGƯỜI DÙNG HIỆN TẠI
        public async Task<APIResponse> getProfile()
        {
            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            if (userCurrent == null)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.Unauthorized;
                response.ErrorMessages = new List<string> { "Người dùng chưa đăng nhập hoặc phiên đăng nhập đã hết hạn." };
                return response;
            }

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userCurrent.Id);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.NotFound;
                response.ErrorMessages = new List<string> { "User is not found!" };
                return response;
            }

            var result = _mapper.Map<ResponseProfileDTO>(user);
            result.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();



            response.IsSuccess = true;
            response.Status = HttpStatusCode.OK;
            response.Result = result;
            return response;
        }

        //lấy ra tất cả nhân viên theo mã chủ sân
        //public async Task<APIResponse> getAllEmployeesByOwner(string maChuSan)
        //{
        //    var employees = await (from nv in _db.NhanViens
        //                           join user in _db.Users on nv.MaNhanVien equals user.Id
        //                           where nv.MaChuSan == maChuSan
        //                           select new EmlpyeeDTO
        //                           {
        //                               MaNhanVien = nv.MaNhanVien,
        //                               Username = user.UserName,
        //                               HoTen = user.HoTen,
        //                               Email = user.Email,
        //                               GioiTinh = user.GioiTinh,
        //                               SoDienThoai = user.PhoneNumber,
        //                               MaSanBong = nv.MaSanBong
        //                           }).ToListAsync();
        //    if (employees == null || !employees.Any())
        //    {
        //        response.IsSuccess = false;
        //        response.Status = HttpStatusCode.NotFound;
        //        response.ErrorMessages = new List<string> { "Không tìm thấy nhân viên." };
        //        return response;
        //    }
        //    response.IsSuccess = true;
        //    response.Status = HttpStatusCode.OK;
        //    response.Result = employees;
        //    return response;
        //}

        public async Task<APIResponse> getAllEmployeesByOwner()
        {
            var userCurrent = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            if (userCurrent == null)
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.Unauthorized;
                response.ErrorMessages = new List<string> { "Người dùng chưa đăng nhập hoặc phiên đăng nhập đã hết hạn." };
                return response;
            }


            var employees = await (from nv in _db.NhanViens
                                   join user in _db.Users on nv.MaNhanVien equals user.Id
                                   where nv.MaChuSan == userCurrent.Id
                                   select new EmlpyeeDTO
                                   {
                                       MaNhanVien = nv.MaNhanVien,
                                       Username = user.UserName,
                                       HoTen = user.HoTen,
                                       Email = user.Email,
                                       GioiTinh = user.GioiTinh,
                                       SoDienThoai = user.PhoneNumber,
                                       MaSanBong = nv.MaSanBong
                                   }).ToListAsync();
            if (employees == null || !employees.Any())
            {
                response.IsSuccess = false;
                response.Status = HttpStatusCode.NotFound;
                response.ErrorMessages = new List<string> { "Không tìm thấy nhân viên." };
                return response;
            }
            response.IsSuccess = true;
            response.Status = HttpStatusCode.OK;
            response.Result = employees;
            return response;
        }
    }
}
