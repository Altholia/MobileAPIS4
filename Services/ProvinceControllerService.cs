using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;

namespace MobileAPIS4.Services;

public class ProvinceControllerService:IProvinceControllerService
{
    private readonly S4Context _context;

    public ProvinceControllerService(S4Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// 返回所有省份的信息
    /// </summary>
    /// <returns>返回分页之后的所有省份信息</returns>
    public Task<PageList<Province>> GetProvincesAsync(ProvinceControllerParameter parameter)
    {
        if(parameter==null)
            throw new ArgumentNullException(nameof(parameter));

        IQueryable<Province> linq= _context.Provinces as IQueryable<Province>;
        return PageList<Province>.CreatePagingAsync(parameter, linq);
    }

    /// <summary>
    /// 根据省份ID查询该省份下的所有城市信息并
    /// </summary>
    /// <param name="provinceId">省份ID，用来确定省份</param>
    /// <returns>返回查询到的所有城市信息</returns>
    public async Task<IEnumerable<City>> GetCityFromProvinceAsync(int provinceId)
    {
        return await _context.Cities.Where(r => r.ProvinceId == provinceId).ToListAsync();
    }

    /// <summary>
    /// 根据 ProvinceId 查看该省份是否存在
    /// </summary>
    /// <param name="provinceId">省份Id，用于查询省份</param>
    /// <returns>存在则返回true ，不存在则返回false</returns>
    public async Task<bool> GetProvinceExistsAsync(int provinceId)
    {
        return await _context.Provinces.AnyAsync(r => r.Id == provinceId);
    }

    /// <summary>
    /// 根据cityId获取所有地区信息
    /// </summary>
    /// <param name="cityId">城市Id，用来查询该城市下的所有地区</param>
    /// <returns>返回查询到的地区</returns>
    public async Task<IEnumerable<District>> GetDistrictFromCityAsync(int cityId)
    {
        return await _context.Districts.Where(r => r.CityId == cityId).ToListAsync();
    }

    /// <summary>
    /// 查看该省份是否存在
    /// </summary>
    /// <param name="cityId">城市ID，用来确定该城市是否存在</param>
    /// <returns>返回true说明该城市存在，否则不存在</returns>
    public async Task<bool> GetCityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(r => r.Id == cityId);
    }
}