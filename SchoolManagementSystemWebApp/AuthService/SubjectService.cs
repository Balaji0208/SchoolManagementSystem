using AutoMapper.Internal;
using SchoolManagementSystemWebApp.AuthService.IService;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using SchoolManagementSystemWebApp.Utility;

namespace SchoolManagementSystemWebApp.AuthService
{
    public class SubjectService : BaseService, ISubjectService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string SchoolUrl;
        public SubjectService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            SchoolUrl = configuration.GetValue<string>("ServiceUrls:SchoolAPI");
        }
        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/SubjectMasterAPIController/GetAllSubject",
                Token = token

            });
        }
        public Task<T> CreateAsync<T>(SubjectMasterDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = SchoolUrl + "/api/SubjectMasterAPIController/Create",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SchoolUrl + "/api/SubjectMasterAPIController/Delete/" + id,
                Token = token
            });
        }
        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SchoolUrl + "/api/SubjectMasterAPIController/GetSubject/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(SubjectMasterDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = SchoolUrl + "/api/SubjectMasterAPIController/Update/" + dto.SubjectId,
                Token = token
            });
        }
    }
}
