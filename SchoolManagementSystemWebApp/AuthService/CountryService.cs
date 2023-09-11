using AutoMapper.Internal;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class CountryService : BaseService, ICountryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public CountryService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/CountryMasterAPI/GetAllCountry",
                Token = token

            });
        }
        public Task<T> CreateAsync<T>(CountryMasterDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = SchoolUrl + "/api/CountryMasterAPI/Create",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Data = id,
                Url = SchoolUrl + "/api/CountryMasterAPI/Delete",
                Token = token
            });
        }
        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data= id,
                Url = SchoolUrl + "/api/CategoryMasterAPI/GetCountry",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(CountryMasterDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SchoolUrl + "/api/CountryMasterAPI/Update" ,
                Token = token
            });
        }
        public Task<T> RecoverAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {

                ApiType = SD.ApiType.PUT,
                Data = id,
                Url = SchoolUrl + "/api/CountryMasterAPI/EnableCountry",//api/AuthApiController/EnableRegistration
                Token = token
            });
        }
    }
}
