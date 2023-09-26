using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IModuleRoleMappingRepository
    {
        Task<ModuleRoleMapping> GetAsync(Expression<Func<ModuleRoleMapping, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<List<ModuleRoleMapping>> GetAllUserAsync(Expression<Func<ModuleRoleMapping, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<ModuleRoleMappingDTO> ModuleRegister(ModuleRoleMappingDTO registration, int userId);
        Task RemoveAsync(ModuleRoleMapping entity, int userId);
        Task<ModuleRoleMapping> UpdateAsync(ModuleRoleMapping entiy, int userId);
       
        
        Task SaveAsync();
    }
}
