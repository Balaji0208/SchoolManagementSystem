using Newtonsoft.Json.Linq;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IUserLoginService
    {
        public Task<UserDTO> GetAllAsync<UserDTO>(string token);
        Task<T> RegisterAsync<T>(UserDTO objToCreate,string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> UpdateAsync<T>(UserDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
      
    }
}
