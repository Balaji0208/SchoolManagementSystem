using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IModuleRoleMappingService
    {

        public Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(ModuleRoleMappingDTO dto, string token);
        Task<T> UpdateAsync<T>(ModuleRoleMappingDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> RecoverAsync<T>(int id, string token);

        Task<T>GetMenuAsync<T>(int id, string token);
     


    }
}
