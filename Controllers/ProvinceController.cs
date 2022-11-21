using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;
using MobileAPIS4.Models.DisplayDto;
using MobileAPIS4.Services;

namespace MobileAPIS4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProvinceController:ControllerBase
{
    private readonly IProvinceControllerService _service;
    private readonly ILogger<ProvinceController> _logger;
    private readonly IMapper _mapper;

    public ProvinceController(IProvinceControllerService service,
        ILogger<ProvinceController> logger,
        IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("province",Name=nameof(GetProvincesAsync))]
    public async Task<ActionResult<IEnumerable<ProvinceDisplayDto>>> GetProvincesAsync(
        [FromQuery] ProvinceControllerParameter parameter)
    {
        PageList<Province> pageList = await _service.GetProvincesAsync(parameter);
        if (pageList.Count == 0)
        {
            _logger.LogWarning($"{nameof(GetProvincesAsync)}查询所有省份信息失败：未获取省份信息");
            return NotFound();
        }

        string previousPageLink = pageList.HasPreviousPage
            ? CreateResourceUriLink(parameter, ResourceUriType.PreviousPage, nameof(GetProvincesAsync))
            : null;
        string nextPageLink = pageList.HasNextPage
            ? CreateResourceUriLink(parameter, ResourceUriType.NextPage, nameof(GetProvincesAsync))
            : null;

        var paging = new
        {
            currentPage = pageList.CurrentPage,
            pageSize = pageList.PageSize,
            totalPage = pageList.TotalPage,
            totalData = pageList.TotalData,
            previousPageLink,
            nextPageLink
        };
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paging,new JsonSerializerOptions
        {
            Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }));

        IEnumerable<ProvinceDisplayDto> provinces = _mapper.Map<IEnumerable<ProvinceDisplayDto>>(pageList);
        return Ok(provinces);
    }

    [HttpGet("citesFromProvince")]
    public async Task<ActionResult<IEnumerable<CityDisplayDto>>> GetCitesFromProvince(
        [FromQuery] int provinceId)
    {
        if (!await _service.GetProvinceExistsAsync(provinceId))
        {
            _logger.LogWarning($"{nameof(GetCitesFromProvince)}请求失败：未找到相关的省份信息！！！");
            return NotFound();
        }

        IEnumerable<City> citiesEntity = await _service.GetCityFromProvinceAsync(provinceId);
        if (!citiesEntity.Any())
        {
            _logger.LogWarning($"{nameof(GetCitesFromProvince)}请求失败：该省份下没有任何的城市信息");
            return NotFound(JsonSerializer.Serialize(citiesEntity));
        }

        IEnumerable<CityDisplayDto> cityDisplayDtos = 
            _mapper.Map<IEnumerable<CityDisplayDto>>(citiesEntity);

        return Ok(cityDisplayDtos);
    }

    [HttpGet("cites/cityId/districts")]
    public async Task<ActionResult<IEnumerable<DistrictDisplayDto>>> GetDistrictsFromCity(
        [FromQuery] int cityId)
    {
        if (!await _service.GetCityExistsAsync(cityId))
        {
            _logger.LogWarning($"{nameof(GetDistrictsFromCity)}请求失败：未找到相关的城市");
            return NotFound();
        }

        IEnumerable<District> districts = await _service.GetDistrictFromCityAsync(cityId);
        if (!districts.Any())
        {
            _logger.LogWarning($"{nameof(GetDistrictsFromCity)}请求失败：该城市下无任何地区信息");
            return NotFound();
        }

        IEnumerable<DistrictDisplayDto> districtDisplayDtos =
            _mapper.Map<IEnumerable<DistrictDisplayDto>>(districts);
        return Ok(districtDisplayDtos);
    }

    [NonAction]
    private string CreateResourceUriLink(ProvinceControllerParameter parameter,
        ResourceUriType type,
        string actionName)
    {
        return type switch
        {
            ResourceUriType.PreviousPage => Url.Link(actionName, new
            {
                currentPage = parameter.CurrentPage - 1,
                pageSize = parameter.PageSize
            }),
            ResourceUriType.NextPage => Url.Link(actionName, new
            {
                currentPage = parameter.CurrentPage + 1,
                pageSize = parameter.PageSize
            }),
            _ => Url.Link(actionName, new
            {
                currentPage = parameter.CurrentPage,
                pageSize = parameter.PageSize
            })
        };
    }
}