using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<Register>> GetAllRegisterAsync(Expression<Func<Register, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO LoginRequestDTO);
        Task<UserDTO> Register(RegistrationDTO registration,int userId);

        Task<List<User>> GetAllUserAsync(Expression<Func<User, bool>> filter = null, bool tracked = true);
        Task<RegistrationDTO> UserRegister(UserDTO registration, int userId);
        Task SaveAsync();
    }
}
