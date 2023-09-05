using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository
{
    public class ClassMasterRepository : IClassMasterRepository 
    {
        private readonly ApplicationDBContext _db;

        public ClassMasterRepository(ApplicationDBContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(ClassMaster entity, int userId)
        {

            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            await _db.AddAsync(entity);
            await SaveAsync();
        }


        public async Task<List<ClassMaster>> GetAllAsync(Expression<Func<ClassMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<ClassMaster> query = _db.ClassMaster;
            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
        //"Villa,VillaSpecial"
        public async Task<ClassMaster> GetAsync(Expression<Func<ClassMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<ClassMaster> query = _db.ClassMaster;
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

        public async Task RemoveAsync(ClassMaster entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.ClassMaster.Update(entity);
            await SaveAsync();

        }
        public async Task<ClassMaster> UpdateAsync(ClassMaster entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.ClassMaster.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();

        }
    }
}
