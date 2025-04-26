using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DATSANBONG.Repository
{
    public class ConfirmRepository : IConfirmRepository
    {
        private readonly ApplicationDbContext _db;
        public ConfirmRepository(ApplicationDbContext db)
        {
            _db = db;
        }
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
    }
}
