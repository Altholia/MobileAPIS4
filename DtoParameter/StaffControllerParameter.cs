using System.ComponentModel.DataAnnotations;

namespace MobileAPIS4.DtoParameter;

/// <summary>
/// Staff的各个参数集合类
/// </summary>
public class StaffControllerParameter
{
    [Display(Name="电话号码")]
    [Required(ErrorMessage="{0}不能为空")]
    [RegularExpression(@"^\d{11}$",ErrorMessage="{0}必须是11位的数字")]
    public string Telephone { get; set; }

    [Display(Name="密码")]
    [Required(ErrorMessage="{0}不能为空")]
    [RegularExpression(@"^\d{6}",ErrorMessage="{0}必须是6位的数字")]
    public string Password { get; set; }
}