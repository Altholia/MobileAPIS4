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
}