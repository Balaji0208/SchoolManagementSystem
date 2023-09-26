using AutoMapper.Internal;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class ModuleService : BaseService, IModuleService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public ModuleService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/MenuAPIController/GetMenus",
                Token = token

            });
        }
        public Task<T> CreateAsync<T>(ModuleDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = SchoolUrl + "/api/MenuAPIController/Create",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Data = id,
                Url = SchoolUrl + "/api/MenuAPIController/DeleteMenu",
                Token = token
            });
        }
        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = id,  
                Url = SchoolUrl + "/api/MenuAPIController/GetMenu",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(ModuleDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SchoolUrl + "/api/MenuAPIController/UpdateMenu",
                Token = token
            });
        }
        public Task<T> RecoverAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {

                ApiType = SD.ApiType.PUT,
                Data = id,
                Url = SchoolUrl + "/api/MenuAPIController/EnableMenu",//api/AuthApiController/EnableRegistration
                Token = token
            });
        }
    }
}
