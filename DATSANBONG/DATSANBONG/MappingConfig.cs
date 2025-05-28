using AutoMapper;
using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using static DATSANBONG.Models.DTO.ResponseEvaluateDTO;

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
            // Mapping từ ApplicationUser sang NguoiDungEvaluateDTO
            CreateMap<ApplicationUser, NguoiDungEvaluateDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.HoTen));

            // Mapping từ DanhGia sang DanhGiaResponseDTO
            CreateMap<DanhGia, ResponseEvaluateDTO>()
                .ForMember(dest => dest.NguoiDung, opt => opt.MapFrom(src => src.NguoiDung));

            CreateMap<DanhGia, ResponseDetailEvaluateDTO>().ReverseMap();
        }
    }
}
