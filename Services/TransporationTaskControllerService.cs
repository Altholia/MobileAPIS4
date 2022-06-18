using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;
using TaskStatus = MobileAPIS4.Entities.TaskStatus;

namespace MobileAPIS4.Services;

public class TransporationTaskControllerService:ITransporationTaskControllerService
{
    private readonly S4Context _context;
    private readonly ILogger<TransporationTaskControllerService> _logger;

    public TransporationTaskControllerService(S4Context context,ILogger<TransporationTaskControllerService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    /// <summary>
    /// 根据TaskId获取对应的运输任务ID
    /// </summary>
    /// <param name="taskId">该参数从查询字符串中获取</param>
    /// <returns>返回查询到了的运输任务信息</returns>
    public async Task<TransporationTask> GetTransporationTaskByIdAsync(int taskId)
    {
        return await _context.TransporationTasks
            .TagWith("根据TaskId获取对应的任务信息")
            .Where(r => r.Id == taskId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 根据StatusId获取对应的TaskStatues的Name属性
    /// </summary>
    /// <param name="statusId">statusId参数</param>
    /// <returns>返回Name属性</returns>
    public async Task<TaskStatus> GetTaskStatusByIdAsync(int statusId)
    {
        _logger.LogError("根据StatusId获取对应的TaskStatues的Name属性");
        return await _context.TaskStatuses.FindAsync(statusId);
    }

    /// <summary>
    /// 获取所有的运输任务，按照日期进行升序
    /// </summary>
    /// <param name="parameter">改参数从查询字符串中获取</param>
    /// <returns>返回分页之后的信息</returns>
    public async Task<PageList<TransporationTask>> GetTotalTransporationTaskAsync(
        TransporationTaskControllerParameter parameter)
    {
        if(parameter==null)
            throw new ArgumentNullException(nameof(parameter));

        IQueryable<TransporationTask> linq = _context.TransporationTasks as IQueryable<TransporationTask>;
        linq = linq.OrderBy(r => r.RequiredCompletionDate);

        return await PageList<TransporationTask>.CreatePagingAsync(parameter, linq);
    }

    /// <summary>
    /// 根据运输状态（StatusId）获取对应的运输任务信息，并按照日期进行升序
    /// </summary>
    /// <param name="parameter">该参数从查询字符串中获取，用于分页操作</param>
    /// <returns>返回所有查询到的结果</returns>
    public async Task<PageList<TransporationTask>> GetNoStartTransporationTaskAsync(
        TransporationTaskControllerParameter parameter)
    {
        if (parameter == null)
            throw new ArgumentNullException(nameof(parameter));

        int statusId = await _context.TaskStatuses
            .Where(r => r.Name == TaskStatusEnum.NoStart.ToString().Trim())
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        IQueryable<TransporationTask> linq=_context.TransporationTasks as IQueryable<TransporationTask>;
        linq = linq.Where(r => r.StatusId == statusId).OrderBy(r=>r.RequiredCompletionDate);

        return await PageList<TransporationTask>.CreatePagingAsync(parameter,linq);
    }

    public async Task<PageList<TransporationTask>> GetOngoingTransporationTaskAsync(
        TransporationTaskControllerParameter parameter)
    {
        if(parameter==null)
            throw new ArgumentNullException(nameof(parameter));

        int statusId = await _context.TaskStatuses
            .Where(r => r.Name == TaskStatusEnum.Ongoing.ToString().Trim())
            .Select(r => r.Id)
            .FirstOrDefaultAsync();
        IQueryable<TransporationTask> linq=_context.TransporationTasks as IQueryable<TransporationTask>;
        linq = linq.Where(r => r.StatusId == statusId).OrderBy(r => r.RequiredCompletionDate);

        return await PageList<TransporationTask>.CreatePagingAsync(parameter, linq);
    }
}