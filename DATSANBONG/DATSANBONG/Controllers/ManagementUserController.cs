using AutoMapper;
using Azure;
using Azure.Core;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DATSANBONG.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class ManagementUserController : Controller
    {
        private readonly IManageUserRepository _manageRepo;
        private readonly IRepository<ApplicationUser> _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        public ManagementUserController(IManageUserRepository manageRepo, IRepository<ApplicationUser> repo,
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _manageRepo = manageRepo;
            this._apiResponse = new APIResponse();
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
        }
        // ADMIN CONFIRM USER
        [HttpPut("confirm_user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<APIResponse>> ConfirmUser(string id, string status)
        {
            var user = await _manageRepo.ConfirmUser(id, status);
            if (user == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid information or user does not exist!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true; ;
            _apiResponse.Result = user;
            return Ok(_apiResponse);
            
        }

        // ADMIN LOCK USER
        [HttpPut("lock_user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<APIResponse>> LockUser([FromBody] ConfirmUserDTO request)
        {
            var user = await _manageRepo.LockUser(request);
            if (user == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Invalid information or user does not exist!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true; ;
            _apiResponse.Result = user;
            return Ok(_apiResponse);

        }

        // ADMIN GET USER BY USERNAME

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin", AuthenticationSchemes ="Bearer")]
        public async Task<ActionResult<APIResponse>> GetUser(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest();
                }
                var user = await _repo.GetAsync(x => x.UserName == username);

                if (user == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Status = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages = new List<string>() { "User not found" };
                    return NotFound(_apiResponse);
                }
                var result = _mapper.Map<ApplicationUserDTO>(user);
                result.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                _apiResponse.IsSuccess = true;
                _apiResponse.Status = HttpStatusCode.OK;
                _apiResponse.Result = result;
                return Ok(_apiResponse);
            }
            catch(Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message};
                return BadRequest(_apiResponse);
            }
        }

        //ADMIN GET ALL USER
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<APIResponse>> GetAllUser()
        {
            var users = await _repo.GetAllAsync();
            if (users == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.NotFound;
                _apiResponse.ErrorMessages = new List<string>() { "User not found" };
                return NotFound(_apiResponse);
            }

            var userDTOs = new List<ApplicationUserDTO>();

            foreach (var user in users)
            {
                var result = _mapper.Map<ApplicationUserDTO>(user);
                result.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                userDTOs.Add(result);
            }
            _apiResponse.IsSuccess = true;
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.Result = userDTOs;
            return Ok(_apiResponse);
        }

        //ADMIN DELETE USER BY USERNAME
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<APIResponse>> DeleteUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Status = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessages = new List<string> { "Invalid user ID" };
                    return BadRequest(_apiResponse);
                }

                var user = await _repo.GetAsync(x => x.Id == id);
                if(user == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Status = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages = new List<string> { "User not found" };
                    return NotFound(_apiResponse);
                }

                await _repo.RemoveAsync(user);
                _apiResponse.Status = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = $"User with ID {id} has been successfully deleted.";
                return Ok(_apiResponse);

            }
            catch(Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_apiResponse);
            }
        }


        // ADMIN LOCK USER (IDENTITY)
        [HttpPost("lock-unlock/{userId}")]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> LockUnlockUser(string userId)
        {
            var response = await _manageRepo.LockUnlockUserAsync(userId);
            return StatusCode((int)response.Status, response);

        }


        // CHỦ SÂN TẠO TÀI KHOẢN NHÂN VIÊN
        [HttpPost("create_employee")]
        [Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddEmployee(EmlpyeeDTO request)
        {
            var response = await _manageRepo.AddEmployee(request);
            return StatusCode((int)response.Status, response);

        }

        // CHỦ SÂN XÓA TÀI KHOẢN NHÂN VIÊN
        [HttpDelete("remove_employee/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var response = await _manageRepo.DeleteEmployee(id);
            return StatusCode((int)response.Status, response);
        }

        // LẤY RA TẤT CẢ NHÂN VIÊN
        [HttpGet("employee_get")]
        [Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var response = await _manageRepo.GetAllEmployees();
            return StatusCode((int)response.Status, response);
        }

        // LẤY RA TẤT CẢ NHÂN VIÊN
        [HttpGet("employee_get/{id}")]
        //[Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetEmployees(string id)
        {
            var response = await _manageRepo.GetEmployee(id);
            return StatusCode((int)response.Status, response);
        }
    }

}
