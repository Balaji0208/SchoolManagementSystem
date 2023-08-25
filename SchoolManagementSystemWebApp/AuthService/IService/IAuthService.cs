using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IAuthService
    {
        public Task<T> GetAllAsync<T>(string token);
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegistrationDTO objToCreate,string token);
    }
}
