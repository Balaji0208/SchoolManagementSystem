using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _db;
        private string secretkey;
        private readonly IMapper _mapper;
   
        public UserRepository(ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, IMapper mapper
            )
        {
            _db = db;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;
            
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;


        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO LoginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == LoginRequestDTO.UserName.ToLower());

            var Role =_db.RoleMaster.Where(u=>u.RoleId==user.RoleId).Select(u=>u.RoleName).FirstOrDefault();
          //  var userid = _db.LocalUsers.Where(u => u.UserName == user.UserName).Select(u => u.UserId).FirstOrDefault(); ;

            if (user == null)
            {
                return null;
            }

         
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, Role),
                     new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())

                 }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user)
               

            };
            return loginResponseDTO;
        }


        public async Task<UserDTO> Register(RegistrationDTO register,int UserId)
        {
          
            try
            {
                User user = new();
                user.UserName = register.UserName;  
                user.Name = register.Name;
                user.Password = register.Password;
                user.RoleId = register.RoleId;
                user.CreatedBy = UserId;
                user.UpdatedBy= UserId;
               


               await  _db.LocalUsers.AddAsync(user);
                await SaveAsync();





            }
            catch (Exception e)
            {
                throw e;

            }

            return new UserDTO();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();

        }


    }
}
