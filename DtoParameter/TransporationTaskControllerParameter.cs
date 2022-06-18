namespace MobileAPIS4.DtoParameter;

public class TransporationTaskControllerParameter
{
    private readonly int _maxPageSize = 20;
    private int _pageSize = 5;

    public int TaskId { get; set; }//运输任务ID
    public int StatusId { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;

        set => _pageSize = value > 0 && value <= _maxPageSize ? value : _pageSize;
    }

}