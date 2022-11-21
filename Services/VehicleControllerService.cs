using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileAPIS4.Entities;

namespace MobileAPIS4.Services;

public class VehicleControllerService:IVehicleControllerService
{
    private readonly S4Context _context;

    public VehicleControllerService(S4Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// 获取所有的空闲车队并按照日期升序
    /// </summary>
    /// <returns>返回查询到的空闲车队</returns>
    public async Task<IEnumerable<Vehicle>> GetFreeVehiclesAsync()
    {
        int statusId = await _context.TaskStatuses
            .TagWith("查询“完成”状态的id是什么")
            .Where(r => r.Name == "Finished")
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        IEnumerable<int> taskStatusIds = await _context.TransporationTasks
            .TagWith("从运输任务表中查询哪些运输任务的状态是“NoStart”和“Ongoing”")
            .Where(r => r.StatusId != statusId)
            .Select(r=>r.Id)
            .ToListAsync();

        IEnumerable<int> vehicleIds = await _context.TaskAssigns
            .TagWith("根据taskStatusId查询“NoStart”和“Ongoing”状态的运输任务分配给了哪些车辆，这些车辆不是空闲的")
            .Where(r => taskStatusIds.Contains(r.TaskId))
            .Select(r => r.VehicleId)
            .Distinct()
            .ToListAsync();

        return await _context.Vehicles.TagWith("查询哪些车辆是空闲的")
            .Where(r => !vehicleIds.Contains(r.Id))
            .Distinct()
            .ToListAsync();
    }

    /// <summary>
    /// 根据吨位自动分配车辆
    /// </summary>
    /// <param name="ton">吨位</param>
    /// <returns>返回符合条件的车辆</returns>
    public async Task<IEnumerable<Vehicle>> GetReasonTonAllocationVehicle(uint ton)
    {
        return await _context.Vehicles
            .TagWith("根据吨位自动分配车辆")
            .Where(r => r.Ton >= ton)
            .ToListAsync();
    }
}