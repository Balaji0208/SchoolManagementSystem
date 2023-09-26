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
    public class ModuleRoleMappingRepository:IModuleRoleMappingRepository
    {
        private readonly ApplicationDBContext _db;
        private string secretkey;
        private readonly IMapper _mapper;

        public ModuleRoleMappingRepository(ApplicationDBContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;

        }
        public async Task<List<ModuleRoleMapping>> GetAllUserAsync(Expression<Func<ModuleRoleMapping, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<ModuleRoleMapping> query = _db.ModuleRoleMapping;

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
        public async Task<ModuleRoleMappingDTO> ModuleRegister(ModuleRoleMappingDTO mapping, int UserId)
        {
            try
            {
                ModuleRoleMapping module = new();
                module.ModuleId = mapping.ModuleId;
                module.RoleId = mapping.RoleId;
                module.CreatedBy = UserId;
                module.UpdatedBy = UserId;
                module.CreatedDate = DateTime.Now;
                module.UpdatedDate = DateTime.Now;


                await _db.ModuleRoleMapping.AddAsync(module);
                await SaveAsync();





            }
            catch (Exception e)
            {
                throw e;

            }

            return new ModuleRoleMappingDTO();
        }
        public async Task<ModuleRoleMapping> GetAsync(Expression<Func<ModuleRoleMapping, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<ModuleRoleMapping> query = _db.ModuleRoleMapping;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
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


            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(ModuleRoleMapping entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.ModuleRoleMapping.Update(entity);
            await SaveAsync();

        }
        public async Task<ModuleRoleMapping> UpdateAsync(ModuleRoleMapping entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;

            _db.ModuleRoleMapping.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        
   
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();

        }



    }
}
