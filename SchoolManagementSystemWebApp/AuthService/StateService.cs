using AutoMapper;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class StateService : BaseService, IStateService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public StateService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/StateMasterAPIController/GetAllCategary",
                Token = token

            });
        }
        public Task<T> CreateAsync<T>(StateMasterDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = SchoolUrl + "/api/StateMasterAPIController/Create",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Data = id,
                Url = SchoolUrl + "/api/StateMasterAPIController/Delete",
                Token = token
            }) ;
        }
        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = id,
                Url = SchoolUrl + "/api/StateMasterAPIController/GetCategary",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(StateMasterDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SchoolUrl + "/api/StateMasterAPIController/Update" ,
                Token = token
            });
        }
        public Task<T> GetStateAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data=id,
                Url = SchoolUrl + "/api/StateMasterAPIController/GetAllStateByCountryId/",
                Token = token
            });
        }
    }
}
