using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;

namespace MobileAPIS4.Models.AddDto;

public class TaskAssignAddDto:IValidatableObject
{
    private readonly S4Context _context;

    public TaskAssignAddDto()
    {
        var scope = ServiceLocator.Instance.CreateScope();
        _context=scope.ServiceProvider.GetRequiredService<S4Context>();
    }

    [Display(Name="运输任务Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int? TaskId { get; set; }

    [Display(Name="车辆ID")]
    [Required(ErrorMessage="{0}不能为空")]
    public int? VehicleId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!_context.TransporationTasks.Any(r => r.Id == TaskId))
            yield return new ValidationResult($"该任务不存在，添加失败", new[] { nameof(TaskId) });

        if (!_context.Vehicles.Any(r => r.Id == VehicleId))
            yield return new ValidationResult($"车辆不存在，添加失败", new[] { nameof(VehicleId) });
    }
}