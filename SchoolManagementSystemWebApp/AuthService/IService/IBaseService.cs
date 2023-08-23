using SchoolManagementSystemWebApp.Models;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
