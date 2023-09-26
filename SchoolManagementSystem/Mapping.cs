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
            CreateMap<LoginRequestDTO, UserDTO>().ReverseMap();
            CreateMap<RegistrationDTO, Register>().ReverseMap();
            CreateMap<CountryMasterDTO, CountryMaster>().ReverseMap();
            CreateMap<ModuleDTO, Module>().ReverseMap();
            CreateMap<StateMasterDTO, StateMaster>().ReverseMap();
            CreateMap<CategoryDTO, Categories>().ReverseMap();
            CreateMap<ModuleRoleMappingDTO, ModuleRoleMapping>().ReverseMap();
            CreateMap<ModuleRoleMapping, User>().ReverseMap();
            CreateMap<RegistrationDTO, UserDTO>().ReverseMap();
        }
    }
}
