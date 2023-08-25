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
using System.Linq.Expressions;
using Microsoft.Win32;

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
        public async Task<List<Register>> GetAllRegisterAsync(Expression<Func<Register, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {



            IQueryable<Register> query = _db.Register;
            
            if (filter != null)
            {
                
                

                query = query.Where(filter);
               

            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }


            return await query.ToListAsync();
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;


        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO LoginRequestDTO)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == LoginRequestDTO.UserName.ToLower());

                var Role = _db.RoleDetails.Where(u => u.RoleId == u.RoleId).Select(u => u.RoleName).FirstOrDefault();
                //var userid = _db.Users.Where(u => u.UserName == user.UserName).Select(u => u.UserId).FirstOrDefault();
                var userRegistrationFromDb = _db.Register.FirstOrDefault(u => u.registerId == user.RegId);

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
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UserDTO> Register(RegistrationDTO register, int UserId)
        {

            try
            {
                Register newRegister = new();


                
                newRegister.FirstName = register.FirstName;
                newRegister.LastName = register.LastName;
                newRegister.Gender = register.Gender;
                newRegister.PhoneNumber = register.PhoneNumber;
                newRegister.DOB=register.DOB;
                newRegister.joiningDate = register.joiningDate;
                newRegister.Address = register.Address;
                newRegister.Email = register.Email;
                newRegister.CreatedBy = UserId;
                newRegister.CreatedDate = DateTime.Now;
                newRegister.UpdatedBy = UserId;
                newRegister.UpdatedDate = DateTime.Now;
                newRegister.categoryId = register.categoryId;
               


                await _db.Register.AddAsync(newRegister);
                await SaveAsync();

                //user.UserName = register.UserName;
                //user.RegId = newRegister.registerId;
                //user.Password = register.UserName + "123";
                //user.CreatedBy = UserId;
                //user.UpdatedBy = UserId;

                //await _db.Users.AddAsync(user);
                //await SaveAsync();





            }
            catch (Exception e)
            {
                throw e;

            }

            return new UserDTO();
        }
        public async Task<List<User>> GetAllUserAsync(Expression<Func<User, bool>> filter = null, bool tracked = true)
        {
            IQueryable<User> query = _db.Users;

            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
        public async Task<RegistrationDTO> UserRegister(UserDTO register, int UserId)
        {
            try
            {
                User user = new();
                user.UserName = register.UserName;
                user.RegId = register.RegId;
                user.Password = register.Password;
                user.RoleId = register.RoleId;
               

                await _db.Users.AddAsync(user);
                await SaveAsync();





            }
            catch (Exception e)
            {
                throw e;

            }

            return new RegistrationDTO();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();

        }



    }
}

