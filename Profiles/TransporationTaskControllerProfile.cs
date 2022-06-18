using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobileAPIS4.Entities;
using MobileAPIS4.Models.DisplayDto;

namespace MobileAPIS4.Profiles;

public class TransporationTaskControllerProfile:Profile
{
    public TransporationTaskControllerProfile()
    {
        CreateMap<TransporationTask, TransporationTaskDisplayDto>()
            .ForMember(des => des.TaskName, rcs => rcs.MapFrom(r => r.Name))
            .ForMember(des => des.TaskId, rcs => rcs.MapFrom(r => r.Id));

        CreateMap<TaskStatus, TransporationTaskDisplayDto>()
            .ForMember(des => des.StatusName, rcs => rcs.MapFrom(r => r.Name));
    }
}