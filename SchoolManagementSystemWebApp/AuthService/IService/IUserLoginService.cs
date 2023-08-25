using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IUserLoginService
    {
        public Task<T> GetAllAsync<T>(string token);
        Task<T> LoginAsync<T>(UserDTO objToCreate);
        
    }
}
