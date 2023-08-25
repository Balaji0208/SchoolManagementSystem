using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface ICategoryService
    {
        Task<CategoriesDTO> GetAllAsync<CategoriesDTO>();
    }
}
