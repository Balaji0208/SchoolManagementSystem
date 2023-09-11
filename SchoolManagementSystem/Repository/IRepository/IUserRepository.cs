using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Expression<Func<User, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<List<User>> GetAllUserAsync(Expression<Func<User, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<RegistrationDTO> UserRegister(UserDTO registration, int userId);
        Task RemoveAsync(User entity, int userId);
        Task<User> UpdateAsync(User entiy, int userId);
        bool IsUniqueUser(string username,int userId);
        
        Task SaveAsync();
    }
}
