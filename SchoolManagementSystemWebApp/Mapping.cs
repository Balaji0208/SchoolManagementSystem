﻿using AutoMapper;
using SchoolManagementSystemWebApp.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolManagementSystemWebApp
{
    public class Mapping : Profile
    {
        public Mapping()
        {
           

            CreateMap<RegistrationDTO, UserDTO>().ReverseMap();
        }

    }
}
