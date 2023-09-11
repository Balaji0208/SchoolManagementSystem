using AutoMapper.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{

    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/AuthApiController/GetAllRegister",
                Token = token

            });
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {

            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = SchoolUrl + "/api/AuthApiController/Login"
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationDTO obj, string token)
        {

            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = SchoolUrl + "/api/AuthApiController/Register",
                Token = token
            });
        }
        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                
                ApiType = SD.ApiType.DELETE,
                Data = id,
                Url = SchoolUrl + "/api/AuthApiController/Remove",
                Token = token
            });
        }
        public Task<T> RecoverAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {

                ApiType = SD.ApiType.PUT,
                Data = id,
                Url = SchoolUrl + "/api/AuthApiController/EnableRegistration",//api/AuthApiController/EnableRegistration
                Token = token
            });
        }
        public Task<T> UpdateAsync<T>(RegistrationDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SchoolUrl + "/api/AuthApiController/Update" ,
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data= id,
                Url = SchoolUrl + "/api/AuthApiController/GetRegisterId",   //api/AuthApiController/GetRegisterId
                Token = token
            });
        }
        public Task<T> GetUserByPrefix<T>(string prefix, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = prefix,
                Url = SchoolUrl + "/api/AuthApiController/GetUserByPrefix",
                Token = token
            });
        }



    }
}

