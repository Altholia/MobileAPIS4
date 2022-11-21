using System.Collections.Generic;
using System.Threading.Tasks;
using MobileAPIS4.Entities;

namespace MobileAPIS4.Services;

public interface IVehicleControllerService
{
    //获取所有的空闲车辆（车牌号升序）
    public Task<IEnumerable<Vehicle>> GetFreeVehiclesAsync();

    //根据运输总量自动分配车辆
    public Task<IEnumerable<Vehicle>> GetReasonTonAllocationVehicle(uint ton);
}