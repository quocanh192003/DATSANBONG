﻿using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG.Repository.IRepository
{
    public interface IManageUserRepository
    {
        Task<ApplicationUser> ConfirmUser  (string id, string status);
        Task<ApplicationUser> LockUser (ConfirmUserDTO request);
        Task<APIResponse> LockUnlockUserAsync(string userId);
        Task<APIResponse> AddEmployee(EmlpyeeDTO request);
        Task<APIResponse> DeleteEmployee(string id);
        Task<APIResponse> GetAllEmployees();
        Task<APIResponse> GetEmployee(string id);
        Task<APIResponse> UpdateInfo(UpdateInfoDTO request);
        Task<APIResponse> getProfile();
        Task<APIResponse> getAllEmployeesByOwner();
    }
}
