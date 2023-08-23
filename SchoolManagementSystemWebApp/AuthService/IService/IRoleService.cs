using SchoolManagementSystemWebApp.Models;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IRoleService
    {
        Task<RoleDetails> GetAllAsync<RoleDetails>();
      
    }
}
