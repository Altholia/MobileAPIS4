using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;
using MobileAPIS4.Models.AddDto;
using MobileAPIS4.Models.DisplayDto;
using MobileAPIS4.Services;
using TaskStatus = MobileAPIS4.Entities.TaskStatus;

namespace MobileAPIS4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransporationTaskController:ControllerBase
{
    private readonly ITransporationTaskControllerService _service;
    private readonly ILogger<TransporationTaskController> _logger;
    private readonly IMapper _mapper;

    public TransporationTaskController(ITransporationTaskControllerService service,
        ILogger<TransporationTaskController> logger,
        IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// 根据TaskId获取对应的运输任务ID
    /// </summary>
    /// <param name="taskId">该参数从查询字符串中获取</param>
    /// <returns>返回查询到了的运输任务信息</returns>
    [HttpGet("TransporationTaskFromId",Name=nameof(GetTransporationTaskByIdAsync))]
    public async Task<ActionResult<TransporationTaskDisplayDto>> GetTransporationTaskByIdAsync([FromQuery]int taskId)
    {
        TransporationTask transporationEntity = await _service.GetTransporationTaskByIdAsync(taskId);

        if (transporationEntity == null)
        {
            _logger.LogWarning(
                $"{nameof(TransporationTaskController)}.{nameof(GetTransporationTaskByIdAsync)}请求失败：未找到相关的实体");
            return NotFound();
        }

        TransporationTaskDisplayDto transporationTaskDisplayDto =
            _mapper.Map<TransporationTaskDisplayDto>(transporationEntity);

        TaskStatus taskStatusEntity = await _service.GetTaskStatusByIdAsync(transporationTaskDisplayDto.StatusId);
        transporationTaskDisplayDto.StatusName = taskStatusEntity.Name;

        return Ok(transporationTaskDisplayDto);
    }

    /// <summary>
    /// 获取所有的运输任务，按照日期进行升序
    /// </summary>
    /// <param name="parameter">改参数从查询字符串中获取</param>
    /// <returns>返回分页之后的信息</returns>
    [HttpGet("TotalTransporationTask",Name=nameof(GetTotalTranspotionTaskAsync))]
    public async Task<ActionResult<IEnumerable<TransporationTaskDisplayDto>>> GetTotalTranspotionTaskAsync(
        [FromQuery]TransporationTaskControllerParameter parameter)
    {

        PageList<TransporationTask> pageList = await _service.GetTotalTransporationTaskAsync(parameter);
        if (pageList.Count == 0)
        {
            _logger.LogWarning(
                $"{nameof(TransporationTaskController)}.{nameof(GetTotalTranspotionTaskAsync)}请求失败：未找到相关的实体");
            return NotFound();
        }

        string previousPageLink = pageList.HasPreviousPage
            ? CreateResourceLink(parameter,ResourceUriType.PreviousPage, nameof(GetTotalTranspotionTaskAsync))
            : null;
        string nextPageLink = pageList.HasNextPage
            ? CreateResourceLink(parameter, ResourceUriType.NextPage, nameof(GetTotalTranspotionTaskAsync))
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

        IEnumerable<TransporationTaskDisplayDto> transporationTaskDisplayDtos =
            _mapper.Map<IEnumerable<TransporationTaskDisplayDto>>(pageList);

        foreach (var transporationTaskDisplayDto in transporationTaskDisplayDtos)
        {
            TaskStatus taskStatusEntity = await _service.GetTaskStatusByIdAsync(transporationTaskDisplayDto.StatusId);
            transporationTaskDisplayDto.StatusName = taskStatusEntity.Name;
        }

        return Ok(transporationTaskDisplayDtos);
    }

    /// <summary>
    /// 获取“未开始”的运输任务
    /// </summary>
    /// <param name="parameter">改参数用于进行分页操作</param>
    /// <returns>返回分页之后的运输任务信息</returns>
    [HttpGet("NotStartTransporationTask")]
    public async Task<ActionResult<IEnumerable<TransporationTaskDisplayDto>>> GetNotStartTransporationTaskAsync(
        [FromQuery]TransporationTaskControllerParameter parameter)
    {
        PageList<TransporationTask> pageList = await _service.GetNoStartTransporationTaskAsync(parameter);
        if (pageList.Count == 0)
        {
            _logger.LogWarning(
                $"{nameof(TransporationTaskController)}.{nameof(GetNotStartTransporationTaskAsync)} 请求失败：未找到相关的实体");
            return NotFound();
        }

        string previousPageLink = pageList.HasPreviousPage
            ? CreateResourceLink(parameter, ResourceUriType.PreviousPage, nameof(GetNotStartTransporationTaskAsync))
            : null;
        string currentPageLink = pageList.HasNextPage
            ? CreateResourceLink(parameter, ResourceUriType.NextPage, nameof(GetNotStartTransporationTaskAsync))
            : null;

        var paging = new
        {
            currentPage = pageList.CurrentPage,
            pageSize = pageList.PageSize,
            totalPage = pageList.TotalPage,
            totalData = pageList.TotalData,
            previousPageLink,
            currentPageLink
        };
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paging,new JsonSerializerOptions
        {
            Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }));

        IEnumerable<TransporationTaskDisplayDto> transDisplaysDto =
            _mapper.Map<IEnumerable<TransporationTaskDisplayDto>>(pageList);

        foreach (var transDisplayDto in transDisplaysDto)
        {
            TaskStatus taskStatus = await _service.GetTaskStatusByIdAsync(transDisplayDto.StatusId);
            transDisplayDto.StatusName = taskStatus.Name;

        }

        return Ok(transDisplaysDto);
    }

    /// <summary>
    /// 获取“正在进行中”的运输任务
    /// </summary>
    /// <param name="parameter">该参数从查询字符串中获取，用来进行分页操作</param>
    /// <returns>返回分页之后的运输信息</returns>
    [HttpGet("OngoingTransporationTask")]
    public async Task<ActionResult<IEnumerable<TransporationTaskDisplayDto>>> GetOngoingTransporationTaskAsync(
        [FromQuery] TransporationTaskControllerParameter parameter)
    {
        PageList<TransporationTask> pageList = await _service.GetOngoingTransporationTaskAsync(parameter);
        if (pageList.Count == 0)
        {
            _logger.LogWarning(
                $"{nameof(TransporationTaskController)}.{nameof(GetOngoingTransporationTaskAsync)} 请求失败：未找到相关的实体");
            return NotFound();
        }

        string previousPageLink = pageList.HasPreviousPage
            ? CreateResourceLink(parameter, ResourceUriType.PreviousPage, nameof(GetOngoingTransporationTaskAsync))
            : null;
        string nextPageLink = pageList.HasNextPage
            ? CreateResourceLink(parameter, ResourceUriType.NextPage, nameof(GetOngoingTransporationTaskAsync))
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
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paging,new JsonSerializerOptions
        {
            Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }));

        IEnumerable<TransporationTaskDisplayDto> transporationTaskDisplayDtos =
            _mapper.Map<IEnumerable<TransporationTaskDisplayDto>>(pageList);

        foreach (var transporationTaskDisplayDto in transporationTaskDisplayDtos)
        {
            TaskStatus taskStatus =
                await _service.GetTaskStatusByIdAsync(transporationTaskDisplayDto.StatusId);
            transporationTaskDisplayDto.StatusName = taskStatus.Name;
        }

        return Ok(transporationTaskDisplayDtos);
    }

    /// <summary>
    /// 获取“已完成”的运输任务信息
    /// </summary>
    /// <param name="parameter">该参数从查询字符串中获取，用来进行分页操作</param>
    /// <returns>返回分页之后的运输信息</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpGet("FinishTransporationTask",Name=nameof(GetFinishTransporationTask))]
    public async Task<ActionResult<IEnumerable<TransporationTaskDisplayDto>>> GetFinishTransporationTask(
        [FromQuery] TransporationTaskControllerParameter parameter)
    {
        PageList<TransporationTask> pageList = await _service.GetFinishedTransporationTaskAsync(parameter);
        if (pageList.Count == 0)
        {
            _logger.LogWarning($"{nameof(GetFinishTransporationTask)} 请求失败：未找到相关数据");
            return NotFound();
        }

        string previousPageLink = pageList.HasPreviousPage
            ? CreateResourceLink(parameter, ResourceUriType.PreviousPage, nameof(GetFinishTransporationTask))
            : null;
        string nextPageLink = pageList.HasNextPage
            ? CreateResourceLink(parameter, ResourceUriType.NextPage, nameof(GetFinishTransporationTask))
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
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paging,new JsonSerializerOptions
        {
            Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }));

        IEnumerable<TransporationTaskDisplayDto> transporationTaskDisplaysDto =
            _mapper.Map<IEnumerable<TransporationTaskDisplayDto>>(pageList);

        foreach (var transporationTaskDisplayDto in transporationTaskDisplaysDto)
        {
            TaskStatus taskStatus = await _service.GetTaskStatusByIdAsync(transporationTaskDisplayDto.StatusId);
            transporationTaskDisplayDto.StatusName = taskStatus.Name;
        }

        return Ok(transporationTaskDisplaysDto);
    }

    /// <summary>
    /// 获取所有的运输状态信息
    /// </summary>
    /// <returns>返回查询的运输状态信息</returns>
    [HttpGet("TotalTaskStatus")]
    public async Task<ActionResult<IEnumerable<TaskStatusDisplayDto>>> GetTotalTaskStatus()
    {
        IEnumerable<TaskStatus> taskStatus = await _service.GetTotalTaskStatusAsync();
        if (taskStatus == null)
        {
            _logger.LogWarning($"{nameof(GetTotalTaskStatus)} 请求失败：未找到相关数据");
            return NotFound();
        }

        IEnumerable<TaskStatusDisplayDto> taskStatusDisplaysDto =
            _mapper.Map<IEnumerable<TaskStatusDisplayDto>>(taskStatus);

        return Ok(taskStatusDisplaysDto);
    }

    /// <summary>
    /// 添加运输任务
    /// </summary>
    /// <param name="addDto">添加的信息</param>
    /// <returns></returns>
    [HttpPost("transporationTask")]
    public async Task<ActionResult<TransporationTaskDisplayDto>> AddTransporationTaskAsync(
        [FromBody] TransporationTaskAddDto addDto)
    {
        TransporationTask transporationTaskEntity = _mapper.Map<TransporationTask>(addDto);

        if (!_service.AddTransporationTaskAsync(transporationTaskEntity))
        {
            _logger.LogError($"{nameof(AddTransporationTaskAsync)}添加失败：运输任务已存在");
            return BadRequest(JsonSerializer.Serialize(
                CustomErrorMessage.CreateErrorMessage(400, "运输任务已存在，请勿重复添加")));
        }

        await _service.SaveAsync();

        TransporationTaskDisplayDto displayDto =
            _mapper.Map<TransporationTaskDisplayDto>(transporationTaskEntity);

        return CreatedAtRoute(nameof(GetTransporationTaskByIdAsync), new{ taskId = displayDto.Id}, displayDto);
    }

    [HttpGet("taskAssigns/{ids}",Name=nameof(GetTaskAssigns))]
    public async Task<ActionResult<IEnumerable<TaskAssignDisplayDto>>> GetTaskAssigns(
        [FromRoute][ModelBinder(BinderType=typeof(TaskAssignModelBinder))] IEnumerable<int> ids)
    {
        if (ids == null)
        {
            _logger.LogError($"{nameof(GetTaskAssigns)}请求失败：参数为空");
            return BadRequest();
        }

        IEnumerable<TaskAssign> taskAssigns = await _service.GetTaskAssignByIdsAsync(ids);
        if (taskAssigns == null||taskAssigns.Count()!=ids?.Count())
        {
            _logger.LogWarning($"{nameof(GetTaskAssigns)}查询失败：没有找到相关的数据");
            return NotFound();
        }

        IEnumerable<TaskAssignDisplayDto> taskAssignsDto =
            _mapper.Map<IEnumerable<TaskAssignDisplayDto>>(taskAssigns);

        return Ok(taskAssignsDto);
    }

    [HttpPost("taskAssign")]
    public async Task<ActionResult<IEnumerable<TaskAssignDisplayDto>>> AddTaskAssignCollection(
        [FromBody] IEnumerable<TaskAssignAddDto> dto)
    {
        IEnumerable<TaskAssign> taskAssigns = _mapper.Map<IEnumerable<TaskAssign>>(dto);

        _service.AddTaskAssignCollection(taskAssigns);
        await _service.SaveAsync();

        IEnumerable<TaskAssignDisplayDto> displayDtos =
            _mapper.Map<IEnumerable<TaskAssignDisplayDto>>(taskAssigns);

        string idsString = string.Join(",",displayDtos.Select(r => r.Id));
        return CreatedAtRoute(nameof(GetTaskAssigns), new{ids=idsString}, displayDtos);
    }

    [HttpGet("transporationTasks/staffId")]
    public async Task<ActionResult<IEnumerable<TransporationTaskDisplayDto>>> GetTransporationTaskByStaffIdAsync(
        [FromQuery] int staffId)
    {
        IEnumerable<TransporationTask> transporationTasks = await _service.GetTransporationTaskByStaffIdAsync(staffId);
        if (!transporationTasks.Any())
        {
            _logger.LogWarning($"{nameof(GetTransporationTaskByStaffIdAsync)}查询失败：没有任何数据");
            return NotFound();
        }

        IEnumerable<TransporationTaskDisplayDto> dtos =
            _mapper.Map<IEnumerable<TransporationTaskDisplayDto>>(transporationTasks);

        return Ok(dtos.OrderBy(r => r.RequiredCompletionDate)); 

    }

    /// <summary>
    /// 生成下一页和上一页的URL
    /// </summary>
    /// <param name="parameter">URL里包含的信息</param>
    /// <param name="type">是下一页还是上一页</param>
    /// <returns></returns>
    private string CreateResourceLink(TransporationTaskControllerParameter parameter,
        ResourceUriType type,
        string actionName)
    {
        return type switch
        {
            ResourceUriType.PreviousPage => Url.Link(actionName, new
            {
                currentPage = parameter.CurrentPage - 1,
                pageSize = parameter.PageSize,
            }),
            ResourceUriType.NextPage => Url.Link(actionName, new
            {
                currentPage = parameter.CurrentPage + 1,
                pageSize = parameter.PageSize,
            }),
            _ => Url.Link(actionName, new
            {
                currentPage = parameter.CurrentPage,
                pageSize = parameter.PageSize,
            })
        };
    }
}