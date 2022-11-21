using System.Security.Cryptography;

namespace MobileAPIS4.DtoParameter;

public class ProvinceControllerParameter
{
    private int _pageSize = 5;
    private int _maxPageSize = 20;

    public int CurrentPage { get; set; } = 1;//当前第几页

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 && value <= _maxPageSize ? value : _pageSize;
    }



}