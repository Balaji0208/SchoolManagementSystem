using SchoolManagementSystem.Models;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface ICategoryRepository
    {

        Task<List<Categories>> GetAllAsync(Expression<Func<Categories, bool>> filter = null, bool tracked = true);
        Task<Categories> GetAsync(Expression<Func<Categories, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Categories entity, int userId);
        Task RemoveAsync(Categories entity, int userId);
        Task<Categories> UpdateAsync(Categories entiy, int userId);
        Task SaveAsync();
    }
}
