using SchoolManagementSystem.Models;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IRoleMasterRepository
    {
        Task<List<RoleDetails>> GetAllAsync(Expression<Func<RoleDetails, bool>> filter = null, bool tracked = true);
        Task<RoleDetails> GetAsync(Expression<Func<RoleDetails, bool>> filter = null, bool tracked = true);
        Task CreateAsync(RoleDetails entity,int userId);
        Task RemoveAsync(RoleDetails entity, int userId);
        Task<RoleDetails> UpdateAsync(RoleDetails entiy,int userId);
        Task SaveAsync();
    }
}
