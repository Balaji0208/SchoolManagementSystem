using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IStateMasterRepository
    {
        Task<List<StateMaster>> GetAllAsync(Expression<Func<StateMaster, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<StateMaster> GetAsync(Expression<Func<StateMaster, bool>> filter = null, bool tracked = true);
        Task CreateAsync(StateMasterDTO entity,int userId);
        Task RemoveAsync(StateMaster entity, int userId);
        Task<StateMaster> UpdateAsync(StateMaster entiy,int userId);
        bool IsUniqueName(string StateName,int StateId);
        Task SaveAsync();
    }
}
