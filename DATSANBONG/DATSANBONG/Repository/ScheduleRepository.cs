using System.Net;
using AutoMapper;
using DATSANBONG.Data;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;


namespace DATSANBONG.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly APIResponse apiResponse;
        public ScheduleRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            this.apiResponse = new APIResponse();
        }
        public async Task<APIResponse> CreatSchedule(LichSanDTO request)
        {
            if (request == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid data" };
                return apiResponse;
            }

            try
            {
                var schedule = _mapper.Map<LichSan>(request);
                var existingSchedule = await _db.LichSans.FirstOrDefaultAsync(x => x.MaLichSan == schedule.MaLichSan);
                if (existingSchedule != null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Status = HttpStatusCode.Conflict;
                    apiResponse.ErrorMessages = new List<string>() { "Schedule already exists" };
                    return apiResponse;
                }

                schedule.TrangThai = "AVAILABLE";
                await _db.LichSans.AddAsync(schedule);
                await _db.SaveChangesAsync();

                var response = _mapper.Map<ResponseScheduleDTO>(schedule);

                apiResponse.IsSuccess = true;
                apiResponse.Result = response;
                apiResponse.Status = HttpStatusCode.Created;

                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return apiResponse;
            }
        }
        public async Task<APIResponse> GetAllSchedule()
        {
            var schedules = await _db.LichSans.ToListAsync();
            if (schedules == null || schedules.Count == 0)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string>() { "No schedules found" };
                return apiResponse;
            }
            var response = _mapper.Map<List<ResponseScheduleDTO>>(schedules);

            apiResponse.IsSuccess = true;
            apiResponse.Result = response;
            apiResponse.Status = HttpStatusCode.OK;
            return apiResponse;

        }
        public async Task<APIResponse> GetScheduleByIdDetailFootball(string masancon)
        {
            if(string.IsNullOrEmpty(masancon))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid ID" };
                return apiResponse;
            }
            var schedule = await _db.LichSans.Where(x => x.MaSanCon == masancon).ToListAsync();
            if (schedule == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string>() { "Schedule not found" };
                return apiResponse;
            }
            var response = _mapper.Map<List<ResponseScheduleDTO>>(schedule);
            apiResponse.IsSuccess = true;
            apiResponse.Result = response;
            apiResponse.Status = HttpStatusCode.OK;
            return apiResponse;
        }
        public async Task<APIResponse> UpdateSchedule(string malichsan, UpdateScheduleDTO request)
        {
            if(string.IsNullOrEmpty(malichsan) || request == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid data" };
                return apiResponse;
            }

            var schedule = await _db.LichSans.FirstOrDefaultAsync(x => x.MaLichSan == malichsan);
            if (schedule == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string>() { "Schedule not found" };
                return apiResponse;
            }

            if (!string.IsNullOrEmpty(request.thu))
                schedule.thu = request.thu;

            if (request.GioBatDau.HasValue)
                schedule.GioBatDau = request.GioBatDau.Value;

            if (request.GioKetThuc.HasValue)
                schedule.GioKetThuc = request.GioKetThuc.Value;

            if (request.GiaThue.HasValue)
                schedule.GiaThue = request.GiaThue.Value;

            if (!string.IsNullOrEmpty(request.TrangThai))
                schedule.TrangThai = request.TrangThai;

            _db.LichSans.Update(schedule);
            await _db.SaveChangesAsync();

            var response = _mapper.Map<ResponseScheduleDTO>(schedule);

            apiResponse.IsSuccess = true;
            apiResponse.Result = response;
            apiResponse.Status = HttpStatusCode.OK;
            return apiResponse;

        }
        public async Task<APIResponse> DeleteSchedule(string malichsan)
        {
            if (string.IsNullOrEmpty(malichsan))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages = new List<string>() { "Invalid ID" };
                return apiResponse;
            }
            var schedule = await _db.LichSans.FirstOrDefaultAsync(x => x.MaLichSan == malichsan);
            if (schedule == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Status = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string>() { "Schedule not found" };
                return apiResponse;
            }
            _db.LichSans.Remove(schedule);
            await _db.SaveChangesAsync();
            apiResponse.IsSuccess = true;
            apiResponse.Status = HttpStatusCode.NoContent;
            return apiResponse;
        }
    }
    
}
