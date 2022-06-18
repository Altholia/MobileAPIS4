using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MobileAPIS4.DtoParameter;
using MobileAPIS4.Entities;

namespace MobileAPIS4.Services;

public class StaffControllerService:IStaffControllerService
{
    private readonly S4Context _context;

    public StaffControllerService(S4Context context)
    {
        _context = context??throw new ArgumentNullException(nameof(context));
    }
    /// <summary>
    /// 通过Telephone和Password获取员工信息
    /// </summary>
    /// <param name="parameter">参数</param>
    /// <returns>返回查询到的Staff实体</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Staff> GetStaffAsync(StaffControllerParameter parameter)
    {
        if (parameter == null)
            throw new ArgumentNullException(nameof(parameter));

        return await _context.Staff
            .TagWith("根据telephone和password获取对应的员工信息")
            .Where(r=>r.Telephone==parameter.Telephone.Trim()&&r.Password==parameter.Password.Trim())
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 根据RoleId获取Role信息
    /// </summary>
    /// <param name="roleId">该参数从路由中获取</param>
    /// <returns>返回查询到的Role实体</returns>
    public async Task<Role> GetRoleAsync(int roleId)
    {
        return await _context.Roles
            .TagWith("根据RoleId获取对应的Role信息")
            .Where(r => r.Id == roleId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}