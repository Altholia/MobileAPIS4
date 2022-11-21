using System;
using System.Collections.Generic;
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

    public TransporationTaskControllerService(S4Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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

    /// <summary>
    /// 获取“正在进行中”的运输任务
    /// </summary>
    /// <param name="parameter">该参数从查询字符串中获取，用来进行分页操作</param>
    /// <returns>返回分页之后的运输信息</returns>
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

    /// <summary>
    /// 获取“已完成”的运输任务信息
    /// </summary>
    /// <param name="parameter">该参数从查询字符串中获取，用来进行分页操作</param>
    /// <returns>返回分页之后的运输信息</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<PageList<TransporationTask>> GetFinishedTransporationTaskAsync(
        TransporationTaskControllerParameter parameter)
    {
        if(parameter==null)
            throw new ArgumentNullException(nameof(parameter));

        int statusId = await _context.TaskStatuses
            .Where(r => r.Name == TaskStatusEnum.Finished.ToString().Trim())
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        IQueryable<TransporationTask> linq= _context.TransporationTasks as IQueryable<TransporationTask>;
        linq = linq.Where(r => r.StatusId == statusId).OrderBy(r => r.RequiredCompletionDate);

        return await PageList<TransporationTask>.CreatePagingAsync(parameter, linq);
    }

    /// <summary>
    /// 获取所有的运输状态信息
    /// </summary>
    /// <returns>返回查询到的运输状态信息</returns>
    public async Task<IEnumerable<TaskStatus>> GetTotalTaskStatusAsync()
    {
        return await _context.TaskStatuses.ToListAsync();
    }

    /// <summary>
    /// 添加TransporationTask（基本信息）
    /// </summary>
    /// <param name="transporationTask">要添加的实体信息</param>
    /// <exception cref="ArgumentNullException"></exception>
    public bool AddTransporationTaskAsync(TransporationTask transporationTask)
    {
        if (transporationTask == null)
            throw new ArgumentNullException(nameof(transporationTask));

        string name = transporationTask.Name;
        if (_context.TransporationTasks.Any(r =>
                r.Name == name && r.RequiredCompletionDate == transporationTask.RequiredCompletionDate))
            return false;

        _context.TransporationTasks.Add(transporationTask);
        return true;
    }

    /// <summary>
    /// 添加任务分配信息
    /// </summary>
    /// <param name="taskAssigns"></param>
    public void AddTaskAssignCollection(IEnumerable<TaskAssign> taskAssigns)
    {
        if (taskAssigns == null)
            throw new ArgumentNullException(nameof(taskAssigns));

        IEnumerable<int> taskIds=taskAssigns.Select(taskAssign => taskAssign.TaskId).ToList();
        if (!_context.TransporationTasks.Any(r => taskIds.Contains(r.Id)))
            return;

        _context.TaskAssigns.AddRange(taskAssigns);
    }

    /// <summary>
    /// 获取任务分配信息
    /// </summary>
    /// <param name="taskIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<IEnumerable<TaskAssign>> GetTaskAssignByIdsAsync(IEnumerable<int> taskIds)
    {
        if (taskIds == null)
            throw new ArgumentNullException(nameof(taskIds));

        return await _context.TaskAssigns
            .TagWith("获取指定的TaskAssigns")
            .Where(r => taskIds.Contains(r.Id))
            .OrderBy(r => r.Id)
            .ToArrayAsync();
    }

    /// <summary>
    /// 根据StaffId获取所有的未完成运输任务
    /// </summary>
    /// <param name="staffId">员工ID</param>
    /// <returns></returns>
    public async Task<IEnumerable<TransporationTask>> GetTransporationTaskByStaffIdAsync(int staffId)
    {
        return await _context.TransporationTasks
            .TagWith("根据StaffId获取所有未完成的运输任务")
            .Where(r => r.VehicleTeamAdministrator == staffId && r.StatusId != ((int)TaskStatusEnum.Finished))
            .ToListAsync();
    }

    public async Task<TransporationTask> UpdateTransporationTaskStatusById(int taskId)
    {
        TransporationTask transporationTask = await _context.TransporationTasks
            .TagWith("根据taskId查询出TransporationTask运输任务进行修改")
            .FirstOrDefaultAsync(r => r.Id == taskId);

        return null;
    }

    /// <summary>
    /// 对进行的所有修改保存到数据库中
    /// </summary>
    /// <returns></returns>
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}