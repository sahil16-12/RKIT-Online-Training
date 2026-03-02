using AutoMapper;
using DoctorAppointmentAPI.DTOs.Appointments;
using DoctorAppointmentAPI.DTOs.Auth;
using DoctorAppointmentAPI.Models;

namespace DoctorAppointmentAPI.Mappers
{
    /// <summary>
    /// Central AutoMapper configuration
    /// </summary>
    public static class MappingProfile
    {
        public static IMapper Mapper;

        static MappingProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // RegisterRequestDto → TBL01
                cfg.CreateMap<RegisterRequestDto, TBL01>()
                   .ForMember(d => d.L01F02, o => o.MapFrom(s => s.Username))
                   .ForMember(d => d.L01F03, o => o.MapFrom(s => s.Password))
                   .ForMember(d => d.L01F04, o => o.MapFrom(s => s.FullName))
                   .ForMember(d => d.L01F05, o => o.MapFrom(s => s.Email))
                   .ForMember(d => d.L01F06, o => o.MapFrom(s => (int)(s.Role ?? UserRole.Patient)));

                // CreateAppointmentRequestDto → TBL03
                cfg.CreateMap<CreateAppointmentRequestDto, TBL03>()
                   .ForMember(d => d.L03F02, o => o.MapFrom(s => s.DoctorId))
                   .ForMember(d => d.L03F04, o => o.MapFrom(s => s.AppointmentDate));

                // TBL03 → AppointmentResponseDto
                cfg.CreateMap<TBL03, AppointmentResponseDto>()
                   .ForMember(d => d.AppointmentId, o => o.MapFrom(s => s.L03F01))
                   .ForMember(d => d.DoctorId, o => o.MapFrom(s => s.L03F02))
                   .ForMember(d => d.PatientId, o => o.MapFrom(s => s.L03F03))
                   .ForMember(d => d.AppointmentDate, o => o.MapFrom(s => s.L03F04))
                   .ForMember(d => d.Status, o => o.MapFrom(s => s.L03F05));
            });

            Mapper = config.CreateMapper();
        }
    }
}