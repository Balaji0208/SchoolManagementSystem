using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IStateService
    {
        Task<T> GetAllAsync<T>(string Token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(StateMasterDTO dto, string token);
        Task<T> UpdateAsync<T>(StateMasterDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T>GetStateAsync<T>(int id, string token);
        Task<T> RecoverAsync<T>(int id, string token);


    }
}
