using SchoolManagementSystem.Models;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IClassMasterRepository
    {
        Task<List<ClassMaster>> GetAllAsync(Expression<Func<ClassMaster, bool>> filter = null, bool tracked = true);
        Task<ClassMaster> GetAsync(Expression<Func<ClassMaster, bool>> filter = null, bool tracked = true);
        Task CreateAsync(ClassMaster entity,int userId);
        Task RemoveAsync(ClassMaster entity, int userId);
        Task<ClassMaster> UpdateAsync(ClassMaster entiy,int userId);
        Task SaveAsync();
    }
}
