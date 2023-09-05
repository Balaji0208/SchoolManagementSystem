using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IClassService
    {
        Task<T> GetAllAsync<T>(string Token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(ClassMasterDTO dto, string token);
        Task<T> UpdateAsync<T>(ClassMasterDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);


    }
}
