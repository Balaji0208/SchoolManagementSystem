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

            public Task<T> LoginAsync<T>(UserDTO obj)
            {

                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.POST,
                    Data = obj,
                    Url = SchoolUrl + "/api/UserApiController/UserRegister"
                });
            }

        }
    }
