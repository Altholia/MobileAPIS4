using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;
using MobileAPIS4.Models.DisplayDto;
using MobileAPIS4.Services;

namespace MobileAPIS4.Controllers;

[ApiController]
[Route("api/{controller}")]
public class StaffController:ControllerBase
{
    private readonly IStaffControllerService _service;
    private readonly ILogger<StaffController> _logger;
    private readonly IMapper _mapper;

    public StaffController(IStaffControllerService service,ILogger<StaffController> logger,IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// 通过Telephone和Password获取员工信息
    /// </summary>
    /// <param name="parameter"> 该参数从查询字符串中获取 </param>
    /// <returns>返回查询到的员工信息</returns>
    [HttpPost("login")]
    public async Task<ActionResult<StaffDisplayDto>> GetStaffAsync(
        [FromBody] StaffControllerParameter parameter)
    {
        Staff staffEntity = await _service.GetStaffAsync(parameter);
        if (staffEntity == null)
        {
            _logger.LogInformation(
                $"{nameof(StaffController)}_{nameof(GetStaffAsync)}：请求的资源不存在——请求参数为：{nameof(StaffControllerParameter.Telephone)}&&{nameof(StaffControllerParameter.Password)}");
            return NotFound();
        }

        _logger.LogInformation(
            $"{nameof(StaffController)}_{nameof(GetStaffAsync)}：请求成功——请求参数为：{nameof(StaffControllerParameter.Telephone)}&&{nameof(StaffControllerParameter.Password)}");

        StaffDisplayDto staffDisplayDto = _mapper.Map<StaffDisplayDto>(staffEntity);
        return Ok(staffDisplayDto);
    }

    /// <summary>
    /// 通过Telephone和Password获取员工信息
    /// </summary>
    /// <param name="parameter">该参数从表单中获取</param>
    /// <returns>返回获取到的员工信息</returns>
    [HttpPost("loginFromForm")]
    public async Task<ActionResult<StaffDisplayDto>> GetStaffFromForm(
        [FromForm] StaffControllerParameter parameter)
    {
        Staff staffEntity = await _service.GetStaffAsync(parameter);
        if (staffEntity == null)
        {
            _logger.LogWarning(
                $"{nameof(StaffController)}_{nameof(GetStaffAsync)}：请求的资源不存在——请求参数为：{parameter.Telephone}&&{parameter.Password}");
            return NotFound();
        }

        _logger.LogInformation(
            $"{nameof(StaffController)}_{nameof(GetStaffAsync)}：请求成功——请求参数为：{parameter.Telephone}&&{parameter.Password}");

        StaffDisplayDto staffDisplayDto = _mapper.Map<StaffDisplayDto>(staffEntity);
        return Ok(staffDisplayDto);
    }

    /// <summary>
    /// 根据roleId获取Role的信息
    /// </summary>
    /// <param name="roleId">从路由中获取该参数</param>
    /// <returns>返回查询到的Role信息</returns>
    [HttpGet("GetRole/{roleId:int}")]
    public async Task<ActionResult<RoleDisplayDto>> GetRoleAsync([FromRoute] int roleId)
    {
        Role roleEntity = await _service.GetRoleAsync(roleId);
        if (roleEntity == null)
        {
            _logger.LogWarning(
                $"{nameof(StaffController)}_{nameof(GetRoleAsync)}：请求的资源不存在——请求参数为：{roleId}");
            return NotFound();
        }

        _logger.LogInformation(
            $"{nameof(StaffController)}_{nameof(GetRoleAsync)}：请求成功——请求参数为：{nameof(roleId)}");

        RoleDisplayDto staffDisplayDto = _mapper.Map<RoleDisplayDto>(roleEntity);
        return Ok(staffDisplayDto);
    }
}