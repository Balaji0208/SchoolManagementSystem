using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using System.Linq.Expressions;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq;

namespace SchoolManagementSystem.Repository
{
  
        public class RoleMasterRepository : IRoleMasterRepository
        {
            private readonly ApplicationDBContext _db;
            
            public RoleMasterRepository(ApplicationDBContext db)
            {
                _db = db;
                
            }
            public async Task CreateAsync(RoleDetails entity,int userId)
            {

                entity.CreatedBy= userId;
                entity.UpdatedBy= userId;
                entity.CreatedDate= DateTime.Now;
                entity.UpdatedDate = DateTime.Now;

            await _db.AddAsync(entity);
                await SaveAsync();
            }


        public async Task<List<RoleDetails>> GetAllAsync(Expression<Func<RoleDetails, bool>> filter = null, bool tracked = true)
        {
            IQueryable<RoleDetails> query = _db.RoleDetails;
            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
            //"Villa,VillaSpecial"
            public async Task<RoleDetails> GetAsync(Expression<Func<RoleDetails, bool>> filter = null, bool tracked = true)
            { 
                IQueryable<RoleDetails> query = _db.RoleDetails;
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

            public async Task RemoveAsync(RoleDetails entity,int userId)
            {

                entity.UpdatedBy = userId;
                entity.UpdatedDate = DateTime.Now;
                _db.RoleDetails.Update(entity);
                await SaveAsync();

            }
        public async Task<RoleDetails> UpdateAsync(RoleDetails entity,int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.RoleDetails.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task SaveAsync()
            {
                await _db.SaveChangesAsync();

            }
        }
    }

