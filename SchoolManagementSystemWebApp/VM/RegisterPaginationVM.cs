﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.VM
{
    public class RegisterPaginationVM
    {

        public RegisterPaginationVM()
        {
            RegisterById = new RegistrationDTO();
        }
        public RegistrationDTO RegisterById { get; set; }

      
        public IEnumerable<RegistrationDTO> Register { get; set; }
       
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }
        public string OrderBy { get; set; }
       
       
    }
}
