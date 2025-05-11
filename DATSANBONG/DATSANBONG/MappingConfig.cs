using AutoMapper;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace DATSANBONG
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, NguoiDungDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
            CreateMap<SanBong, SanBongDTO>().ReverseMap();
            CreateMap<CreateDetailFootballDTO, ChiTietSanBong>().ReverseMap();
            CreateMap<ChiTietSanBong, ResponseDetailFootballDTO>().ReverseMap();
            CreateMap<LichSan, LichSanDTO>().ReverseMap();
            CreateMap<LichSan, ResponseScheduleDTO>().ReverseMap();
            CreateMap<LichSan, UpdateScheduleDTO>().ReverseMap();
            CreateMap<DonDatSan, ResponseOrderDTO>().ReverseMap();
            CreateMap<ChiTietDonDatSan, ChiTietDonDatSanDTO>().ReverseMap();
            CreateMap<HinhAnh, ResponseHinhAnhDTO>().ReverseMap();
            CreateMap<SanBong, ResponseSanBongDTO>()
                .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnhs))
                .ReverseMap();
        }
    }
}
