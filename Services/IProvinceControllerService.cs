using System.Collections.Generic;
using System.Threading.Tasks;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;

namespace MobileAPIS4.Services;

public interface IProvinceControllerService
{
    //获取所有省份信息
    public Task<PageList<Province>> GetProvincesAsync(ProvinceControllerParameter parameter);

    //根据省份信息获取该省份下的所有城市信息
    public Task<IEnumerable<City>> GetCityFromProvinceAsync(int provinceId);

    //根据 provinceId 查看该省份是否存在
    public Task<bool> GetProvinceExistsAsync(int provinceId);

    //根据cityId获取地区信息
    public Task<IEnumerable<District>> GetDistrictFromCityAsync(int cityId);

    //根据 cityID 查看该城市是否存在
    public Task<bool> GetCityExistsAsync(int cityId);
}