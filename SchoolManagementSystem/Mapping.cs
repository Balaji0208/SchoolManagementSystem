using AutoMapper;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;

namespace SchoolManagementSystem
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<RoleDetails, RoleDetailsDTO>().ReverseMap();

            CreateMap<RegistrationDTO, UserDTO>().ReverseMap();
        }
    }
}
