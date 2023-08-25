using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public CategoryService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/CategoryMasterAPI/GetAllCategary"
                // Token = token

            });
        }
       

    }
}

