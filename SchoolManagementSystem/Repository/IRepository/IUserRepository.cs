using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO LoginRequestDTO);
        Task<UserDTO> Register(RegistrationDTO registration,int userId);
        Task SaveAsync();
    }
}
