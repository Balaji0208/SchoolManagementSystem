using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDBContext _db;
        private string secretkey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _db = db;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;

        }
        public async Task<List<User>> GetAllUserAsync(Expression<Func<User, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<User> query = _db.Users;

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
        public async Task<RegistrationDTO> UserRegister(UserDTO register, int UserId)
        {
            try
            {
                User user = new();
                user.UserName = register.UserName;
                user.registerId = register.registerId;
                user.Password = register.Password;
                user.RoleId = register.RoleId;
                user.CreatedBy = UserId;
                user.UpdatedBy = UserId;
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;


                await _db.Users.AddAsync(user);
                await SaveAsync();





            }
            catch (Exception e)
            {
                throw e;

            }

            return new RegistrationDTO();
        }
        public async Task<User> GetAsync(Expression<Func<User, bool>> filter = null, bool tracked = true)
        {
            IQueryable<User> query = _db.Users;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }



            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(User entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.Users.Update(entity);
            await SaveAsync();

        }
        public async Task<User> UpdateAsync(User entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;

            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public bool IsUniqueUser(string username,int userId)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == username && x.UserId != userId);
            if (user == null)
            {
                return true;
            }
            return false;


        }
   
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();

        }



    }
}
