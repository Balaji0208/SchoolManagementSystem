using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using System.Linq.Expressions;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq;

namespace SchoolManagementSystem.Repository
{
  
        public class ModuleRepository : IModuleRepository
    {
            private readonly ApplicationDBContext _db;
            
            public ModuleRepository(ApplicationDBContext db)
            {
                _db = db;
                
            }
            public async Task CreateAsync(Module entity,int moduleId)
            {

                entity.CreatedBy= moduleId;
                entity.UpdatedBy= moduleId;
                entity.CreatedDate= DateTime.Now;
                entity.UpdatedDate = DateTime.Now;

            await _db.AddAsync(entity);
                await SaveAsync();
            }


        public async Task<List<Module>> GetAllAsync(Expression<Func<Module, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Module> query = _db.Module;
            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
            //"Villa,VillaSpecial"
            public async Task<Module> GetAsync(Expression<Func<Module, bool>> filter = null, bool tracked = true)
            { 
                IQueryable<Module> query = _db.Module;
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

            public async Task RemoveAsync(Module entity,int userId)
            {

                entity.UpdatedBy = userId;
                entity.UpdatedDate = DateTime.Now;
                _db.Module.Update(entity);
                await SaveAsync();

            }
        public async Task<Module> UpdateAsync(Module entity,int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.Module.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public bool IsUniqueName(string ModuleName, int ModuleId)
        {
            var user = _db.Module.FirstOrDefault(x => x.Menus == ModuleName && x.ModuleId!= ModuleId);
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

