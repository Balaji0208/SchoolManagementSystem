using AutoMapper;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class ModuleRoleMappingService : BaseService, IModuleRoleMappingService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public ModuleRoleMappingService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/ModuleRoleMappingController/GetAllModuleRoles",
                Token = token

            });
        }


        public Task<T> GetMenuAsync<T>(int roleId,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data=roleId,
                Url = SchoolUrl + "/api/ModuleRoleMappingController/GetAllMenuByRoleId/",
                Token = token
            });;
        }
        public Task<T> CreateAsync<T>(ModuleRoleMappingDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = SchoolUrl + "/api/ModuleRoleMappingController/moduleRegister",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Data = id,
                Url = SchoolUrl + "/api/ModuleRoleMappingController/DeleteRoleModule",
                Token = token
            });
        }
        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = id,
                Url = SchoolUrl + "/api/ModuleRoleMappingController/GetAllModuleRole",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(ModuleRoleMappingDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SchoolUrl + "/api/ModuleRoleMappingController/Update",
                Token = token
            });
        }
        public Task<T> RecoverAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {

                ApiType = SD.ApiType.PUT,
                Data = id,
                Url = SchoolUrl + "/api/ModuleRoleMappingController/Enable",//api/AuthApiController/EnableRegistration
                Token = token
            });
        }

    }
}
