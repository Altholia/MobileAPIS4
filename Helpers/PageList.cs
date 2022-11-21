using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileAPIS4.DtoParameter;

namespace MobileAPIS4.Helpers;

public class PageList<T>:List<T>
{
    public int TotalPage { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalData { get; private set; }
    public bool HasNextPage => CurrentPage < TotalPage;
    public bool HasPreviousPage => CurrentPage > 1;

    public PageList(List<T> items,int currentPage,int pageSize,int totalData)
    {
        TotalPage = (int)Math.Ceiling(totalData / (double)pageSize);
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalData = totalData;
        AddRange(items);
    }

    public static async Task<PageList<T>> CreatePagingAsync(
        TransporationTaskControllerParameter parameter,
        IQueryable<T> linq)
    {
        int totalData = await linq.CountAsync();
        var items = await linq.TagWith("根据TaskId获取TransporationTask的信息，并进行分页")
            .Skip(parameter.PageSize * (parameter.CurrentPage - 1))
            .Take(parameter.PageSize)
            .ToListAsync();
            
        return new PageList<T>(items, parameter.CurrentPage, parameter.PageSize, totalData);
    }

    public static async Task<PageList<T>> CreatePagingAsync(
        ProvinceControllerParameter parameter,
        IQueryable<T> linq)
    {
        int totalData = await linq.CountAsync();
        var items = await linq.TagWith("根据TaskId获取TransporationTask的信息，并进行分页")
            .Skip(parameter.PageSize * (parameter.CurrentPage - 1))
            .Take(parameter.PageSize)
            .ToListAsync();

        return new PageList<T>(items, parameter.CurrentPage, parameter.PageSize, totalData);
    }
}