using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IAuthService
    {
        public Task<T> GetAllAsync<T>(string token);
        Task<T> RecoverAsync<T>(int id,string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegistrationDTO objToCreate,string token);
        Task<T> UpdateAsync<T>(RegistrationDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> GetUserByPrefix<T>(string prefix, string token);

    }
}
