using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task<List<Register>> GetAllRegisterAsync(Expression<Func<Register, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<Register> GetAsync(Expression<Func<Register, bool>> filter = null, bool tracked = true);
        bool IsUniqueUser(string username,int userId);
        Task<LoginResponseDTO> Login(LoginRequestDTO LoginRequestDTO);
        Task<UserDTO> Register(RegistrationDTO registration, int userId);

        Task RemoveAsync(Register entity, int userId);
        Task<Register> UpdateAsync(Register entiy, int userId);
        Task<List<Register>> GetUserByPrefix(string prefix);

        Task SaveAsync();
    }
}
