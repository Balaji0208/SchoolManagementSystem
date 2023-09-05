using SchoolManagementSystem.Models;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface ICountryMasterRepository
    {
        Task<List<CountryMaster>> GetAllAsync(Expression<Func<CountryMaster, bool>> filter = null, bool tracked = true);
        Task<CountryMaster> GetAsync(Expression<Func<CountryMaster, bool>> filter = null, bool tracked = true);
        Task CreateAsync(CountryMaster entity,int userId);
        Task RemoveAsync(CountryMaster entity, int userId);
        Task<CountryMaster> UpdateAsync(CountryMaster entiy,int userId);
        bool IsUniqueName(string CountryName,int CountryId);

        Task SaveAsync();
    }
}
