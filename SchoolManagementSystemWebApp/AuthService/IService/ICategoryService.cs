using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface ICategoryService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(CategoriesDTO dto, string token);
        Task<T> UpdateAsync<T>(CategoriesDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
