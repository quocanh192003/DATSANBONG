using AutoMapper;
using DATSANBONG.Data;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATSANBONG.Repository
{
    public class ConfirmFootballRepository : IConfirmFootballRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ConfirmFootballRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<SanBongDTO> confirmFootball(string Id, string status)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(status) || status.ToUpper() != "ACTIVE")
            {
                return null;
            }
            var football = await _db.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == Id);
            if(football == null)
            {
                return null;
            }

            if(football.TrangThai.ToUpper() != "PENDING")
            {
                return null;
            }

            football.TrangThai = status.ToUpper();
            _db.SanBongs.Update(football);
            await _db.SaveChangesAsync();
            return _mapper.Map<SanBongDTO>(football);
        }

        public async Task<SanBongDTO> GetFootballById(string Id)
        {
            //if (string.IsNullOrWhiteSpace(Id))
            //{
            //    return null;
            //}

            var foot = await _db.SanBongs.FirstOrDefaultAsync(x => x.MaSanBong == Id);
            if (foot == null)
            {
                return null;
            }

            return _mapper.Map<SanBongDTO>(foot);
        }

        public async Task<List<SanBongDTO>> GetAllFootball()
        {
            var football = await _db.SanBongs.Where(x => x.TrangThai.ToUpper() == "PENDING").ToListAsync();
            if (football == null)
            {
                return null;
            }
            return _mapper.Map<List<SanBongDTO>>(football);
        }
    }
}
