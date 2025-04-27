using AutoMapper;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DATSANBONG.Repository
{
    public class ManageUserRepository : IManageUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ManageUserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        //ADMIN COMFIRM USER
        public async Task<ApplicationUser> ConfirmUser(ConfirmUserDTO request)
        {
            if (request == null || request.TrangThai.ToUpper() != "ACTIVE")
            {
                return null;
            }

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x =>x.Id == request.ID);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (user.TrangThai.ToUpper() == "PENDING")
                {
                    user.TrangThai = request.TrangThai.ToUpper();
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
            if(status != "INACTIVE")
            {
                return null;
            }

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.ID);
            if(user == null)
            {
                return null;
            }
            else
            {
                if(user.TrangThai.ToUpper() == "ACTIVE")
                {
                    user.TrangThai = status;
                    _db.ApplicationUsers.Update(user);
                    await _db.SaveChangesAsync();
                    return user;
                }
                return null;
            }
        }

                
    }
}
