using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobileAPIS4.Entities;
using MobileAPIS4.Models.AddDto;
using MobileAPIS4.Models.DisplayDto;

namespace MobileAPIS4.Profiles;

public class TransporationTaskControllerProfile:Profile
{
    public TransporationTaskControllerProfile()
    {
        CreateMap<TransporationTask, TransporationTaskDisplayDto>();
        CreateMap<TaskStatus, TaskStatusDisplayDto>();
        CreateMap<TransporationTaskAddDto, TransporationTask>();
        CreateMap<TaskAssign, TaskAssignDisplayDto>();
        CreateMap<TaskAssignAddDto, TaskAssign>();
    }
}