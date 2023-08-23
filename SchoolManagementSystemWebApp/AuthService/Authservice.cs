using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
   
        public class AuthService : BaseService,IAuthService
        {
            private readonly IHttpClientFactory _clientFactory;
            private string SchoolUrl;
            public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
            {
                _clientFactory = clientFactory;
                SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
            }

            public Task<T> LoginAsync<T>(LoginRequestDTO obj)
            {

                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.POST,
                    Data = obj,
                    Url = SchoolUrl + "/api/UserApiController/Login"
                });
            }

            public Task<T> RegisterAsync<T>(RegistrationDTO obj, string token)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.POST,
                    Data = obj,
                    Url = SchoolUrl + "/api/UserApiController/Register",
                    Token = token
                });
            }
        }
    }

