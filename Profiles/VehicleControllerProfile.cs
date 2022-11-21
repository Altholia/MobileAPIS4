using AutoMapper;
using MobileAPIS4.Entities;
using MobileAPIS4.Models.DisplayDto;

namespace MobileAPIS4.Profiles;

public class VehicleControllerProfile:Profile
{
    public VehicleControllerProfile()
    {
        CreateMap<Vehicle, VehicleDisplayDto>();
    }
}