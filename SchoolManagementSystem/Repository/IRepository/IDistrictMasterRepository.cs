using SchoolManagementSystem.Models;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IDistrictMasterRepository
    {
        Task<List<DistrictMaster>> GetAllAsync(Expression<Func<DistrictMaster, bool>> filter = null, bool tracked = true);
        Task<DistrictMaster> GetAsync(Expression<Func<DistrictMaster, bool>> filter = null, bool tracked = true);
        Task CreateAsync(DistrictMaster entity,int userId);
        Task RemoveAsync(DistrictMaster entity, int userId);
        Task<DistrictMaster> UpdateAsync(DistrictMaster entiy,int userId);
        bool IsUniqueName(string DistrictName);

        Task SaveAsync();
    }
}
