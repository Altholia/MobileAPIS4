using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPIS4.Entities;
using MobileAPIS4.Models.DisplayDto;
using MobileAPIS4.Services;

namespace MobileAPIS4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController:ControllerBase
{
    private readonly IVehicleControllerService _service;
    private readonly ILogger<VehicleController> _logger;
    private readonly IMapper _mapper;

    public VehicleController(
        IVehicleControllerService service,
        ILogger<VehicleController> logger,
        IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// 查询空闲的车队并按照日期升序
    /// </summary>
    /// <returns>返回排序之后的空闲车队信息</returns>
    [HttpGet("freeVehicles")]
    public async Task<ActionResult<IEnumerable<VehicleDisplayDto>>> GetFreeVehiclesAsync()
    {
        IEnumerable<Vehicle> vehicleEntity = await _service.GetFreeVehiclesAsync();
        if (!vehicleEntity.Any())
        {
            _logger.LogWarning($"{nameof(GetFreeVehiclesAsync)}请求失败：没有找到空闲的车队");
            return NotFound();
        }

        IEnumerable<VehicleDisplayDto> vehicles = 
            _mapper.Map<IEnumerable<VehicleDisplayDto>>(vehicleEntity);
        return Ok(vehicles.OrderBy(r=>r.PlateNumber));
    }

    /// <summary>
    /// 根据吨位自动分配车辆（将会查询出所有大于等于该吨位的车辆）并按照吨位进行升序
    /// </summary>
    /// <param name="ton">吨位</param>
    /// <returns>返回符合条件的车辆</returns>
    [HttpGet("ton/vehicles")]
    public async Task<ActionResult<IEnumerable<VehicleDisplayDto>>> GetReasonTonAllocationVehicle(
        [FromQuery]uint ton)
    {
        IEnumerable<Vehicle> vehiclesEntity = await _service.GetReasonTonAllocationVehicle(ton);
        if (!vehiclesEntity.Any())
        {
            _logger.LogWarning($"{nameof(GetReasonTonAllocationVehicle)}请求失败：没有找到相关的车辆");
            return NotFound();
        }

        IEnumerable<VehicleDisplayDto> vehiclesDisplayDtos =
            _mapper.Map<IEnumerable<VehicleDisplayDto>>(vehiclesEntity);
        return Ok(vehiclesDisplayDtos.OrderBy(r=>r.Ton));
    }
}