using SchoolManagementSystem.Models;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IModuleRepository
    {
        Task<List<Module>> GetAllAsync(Expression<Func<Module, bool>> filter = null, bool tracked = true);
        Task<Module> GetAsync(Expression<Func<Module, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Module entity, int ModuleId);
        Task RemoveAsync(Module entity, int ModuleId);
        Task<Module> UpdateAsync(Module entiy, int ModuleId);
        bool IsUniqueName(string ModuleName, int ModuleId);

        Task SaveAsync();
    }
}
