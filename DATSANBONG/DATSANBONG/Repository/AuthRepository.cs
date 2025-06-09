using AutoMapper;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using DATSANBONG.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace DATSANBONG.Repository
{
    public class AuthRepository : IAuthRepositoty
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string secretKey;
        private readonly IEmailService _emailService;
        public AuthRepository(ApplicationDbContext db, IConfiguration configuration, 
            UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSetting:Secret");
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public bool IsUniqueUser(string TaiKhoan)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == TaiKhoan);
            if (user == null)
            {
                return true; 
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            var user =_db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == model.Username.ToLower());
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }
            if (user.TrangThai != "ACTIVE" || !user.EmailConfirmed)
            {
                return null;
            }



            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }

            //If user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<NguoiDungDTO>(user),
                Role = roles.FirstOrDefault(),
            };
            return loginResponseDTO;
        }

        public async Task<NguoiDungDTO> Register(RegisterRequestDTO model)
        {

            // Kiểm tra email
            if (!new EmailAddressAttribute().IsValid(model.Email))
                throw new ArgumentException("Email không hợp lệ.");

            var user = new ApplicationUser
            {
                UserName = model.Username,
                HoTen = model.HoTen,
                Email = model.Email,
                NgaySinh = model.NgaySinh,
                TrangThai = "PENDING",
                GioiTinh = model.GioiTinh,
                PhoneNumber = model.SoDienThoai,
                NormalizedEmail = model.Username.ToUpper()
            };

            // Tạo người dùng
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Lỗi tạo tài khoản: {errors}");
            }

            // Tạo roles nếu chưa có
            await CreateRolesIfNotExistsAsync();

            // Gán vai trò
            var roleName = model.TenVaiTro.Trim().ToUpper();
            var roleAssignResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleAssignResult.Succeeded)
            {
                string errors = string.Join(", ", roleAssignResult.Errors.Select(e => e.Description));
                throw new Exception($"Lỗi gán vai trò: {errors}");
            }

            // Nếu là ADMIN, tự động active
            if (roleName == "ADMIN")
            {
                user.TrangThai = "ACTIVE";
                _db.ApplicationUsers.Update(user);
                await _db.SaveChangesAsync();
            }

            // Gửi email xác nhận
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmail(user.Email, "Xác nhận email", code);

            return new NguoiDungDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                ConfirmationMessage = "Hãy kiểm tra email để xác nhận tài khoản."
            };
        }

        private async Task CreateRolesIfNotExistsAsync()
        {
            var roles = new[] { "ADMIN", "CHỦ SÂN", "KHÁCH HÀNG", "NHÂN VIÊN" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        public async Task<ResponseTokenPasswordDTO> ForgotPassword(RequestForgotPasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new ResponseTokenPasswordDTO();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(token))
                return new ResponseTokenPasswordDTO();
            //send email
            await _emailService.SendEmail(user.Email, "Code to reset your password", token);

            return new ResponseTokenPasswordDTO()
            {
                Email = user.Email,
                Token = token
            };
        }


    }
}
