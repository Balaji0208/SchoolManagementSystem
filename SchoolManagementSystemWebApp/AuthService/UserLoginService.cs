using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class UserLoginService : BaseService, IUserLoginService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public UserLoginService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/UserApiController/GetAllUserLogin",
                Token = token

            });
        }

        public Task<T> RegisterAsync<T>(UserDTO obj, string token)
        {

            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = SchoolUrl + "/api/UserApiController/UserRegister",
                Token = token
            });
        }
        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Data = id,
                Url = SchoolUrl + "/api/UserApiController/RemoveUser",
                Token = token
            }); 
        }
        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = id,
                Url = SchoolUrl + "/api/UserApiController/GetUserId",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(UserDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SchoolUrl + "/api/UserApiController/UpdateUser",
                Token = token
            });
        }

       
        
    }
}
