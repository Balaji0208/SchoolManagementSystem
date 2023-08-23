using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public RoleService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/RoleMasterAPI/GetRols",
               // Token = token

            });
        }
    }
}
