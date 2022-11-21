using AutoMapper;
using MobileAPIS4.Entities;
using MobileAPIS4.Models.DisplayDto;

namespace MobileAPIS4.Profiles;

public class ProvinceControllerProfile:Profile
{
    public ProvinceControllerProfile()
    {
        CreateMap<Province, ProvinceDisplayDto>();
        CreateMap<City, CityDisplayDto>();
        CreateMap<District, DistrictDisplayDto>();
    }
}