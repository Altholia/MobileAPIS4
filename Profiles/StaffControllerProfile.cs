using AutoMapper;
using MobileAPIS4.Entities;
using MobileAPIS4.Models.DisplayDto;

namespace MobileAPIS4.Profiles;

public class StaffControllerProfile:Profile
{
    public StaffControllerProfile()
    {
        CreateMap<Staff, StaffDisplayDto>()
            .ForMember(scr => scr.Name, des => des.MapFrom(r => $"{r.FirstName} {r.LastName}"))
            .ForMember(scr => scr.Gender, des => des.MapFrom(r => r.Gender.ToLower() == "f" ? "女" : "男"));

        CreateMap<Role, RoleDisplayDto>();
    }
}