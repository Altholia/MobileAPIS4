using System;
using System.ComponentModel.DataAnnotations;

namespace MobileAPIS4.Models.AddDto;

public class TransporationTaskAddDto
{
    [Display(Name = "运输任务名字")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Name { get; set; }

    [Display(Name = "状态ID")]
    [Range(1, 3, ErrorMessage = "{0}的取值范围在{1}到{2}之间")]
    public int StatusId { get; set; } = 1;

    [Display(Name = "开始区域的ID")]
    [Required(ErrorMessage = "{0}不能为null")]
    public int StartDistrictId { get; set; }

    [Display(Name = "开始街道的名字")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string StartStreetAddress { get; set; }

    [Display(Name = "结束区域的ID")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int DestinationDistrictId { get; set; }

    [Display(Name = "结束街道的名字")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string DestinationStreetAddress { get; set; }

    [Display(Name = "承重量")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal Weight { get; set; }

    [Display(Name = "完成任务时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime RequiredCompletionDate { get; set; }

    [Display(Name = "车队管理人员")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int VehicleTeamAdministrator { get; set; }

    [Display(Name = "备注")]
    public string Remark { get; set; }
}