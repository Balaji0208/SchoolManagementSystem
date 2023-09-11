﻿using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.AuthService.IService
{
    public interface ICountryService
    {
        Task<T> GetAllAsync<T>(string Token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(CountryMasterDTO dto, string token);
        Task<T> UpdateAsync<T>(CountryMasterDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> RecoverAsync<T>(int id, string token);


    }
}
