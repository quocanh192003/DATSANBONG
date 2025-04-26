using AutoMapper;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;

namespace DATSANBONG
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<ApplicationUser, NguoiDungDTO>().ReverseMap();
        }
    }
}
