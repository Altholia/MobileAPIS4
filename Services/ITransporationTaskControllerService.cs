using System.Collections.Generic;
using System.Threading.Tasks;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;
using TaskStatus = MobileAPIS4.Entities.TaskStatus;

namespace MobileAPIS4.Services;

public interface ITransporationTaskControllerService
{
    //根据运输任务ID获取运输任务的基本信息
    public Task<TransporationTask> GetTransporationTaskByIdAsync(int taskId);

    //根据StatusId获取TaskStatus的Name属性
    public Task<TaskStatus> GetTaskStatusByIdAsync(int statusId);

    //获取所有运输任务，并按照日期升序
    public Task<PageList<TransporationTask>> GetTotalTransporationTaskAsync(
        TransporationTaskControllerParameter parameter);

    //获取“未开始”的运输任务
    public Task<PageList<TransporationTask>> GetNoStartTransporationTaskAsync(
        TransporationTaskControllerParameter parameter);

    //获取“进行中”的运输任务
    public Task<PageList<TransporationTask>> GetOngoingTransporationTaskAsync(
        TransporationTaskControllerParameter parameter);

    //获取“已完成”的运输任务
    public Task<PageList<TransporationTask>> GetFinishedTransporationTaskAsync(
        TransporationTaskControllerParameter parameter);

    //获取所有的运输状态
    public Task<IEnumerable<TaskStatus>> GetTotalTaskStatusAsync();

    //添加运输任务1（基本信息）
    public bool AddTransporationTaskAsync(TransporationTask transporationTask);

    //添加运输任务2（任务分配信息）
    public void AddTaskAssignCollection(IEnumerable<TaskAssign> taskAssign);

    //获取任务分配信息
    public Task<IEnumerable<TaskAssign>> GetTaskAssignByIdsAsync(IEnumerable<int> taskIds);

    //根据StaffId获取所有未完成的运输任务
    public Task<IEnumerable<TransporationTask>> GetTransporationTaskByStaffIdAsync(int staffId);

    //根据运输任务ID修改他的任务状态
    public Task<TransporationTask> UpdateTransporationTaskStatusById(int taskId);

    //保存数据至数据库中
    public Task<bool> SaveAsync();
}