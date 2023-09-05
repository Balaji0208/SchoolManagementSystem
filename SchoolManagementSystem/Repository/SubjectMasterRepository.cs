using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository
{
    public class SubjectMasterRepository : ISubjectMasterRepository 
    {
        private readonly ApplicationDBContext _db;

        public SubjectMasterRepository(ApplicationDBContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(SubjectMaster entity, int userId)
        {

            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            await _db.AddAsync(entity);
            await SaveAsync();
        }


        public async Task<List<SubjectMaster>> GetAllAsync(Expression<Func<SubjectMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<SubjectMaster> query = _db.SubjectMaster;
            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
        //"Villa,VillaSpecial"
        public async Task<SubjectMaster> GetAsync(Expression<Func<SubjectMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<SubjectMaster> query = _db.SubjectMaster;
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

        public async Task RemoveAsync(SubjectMaster entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.SubjectMaster.Update(entity);
            await SaveAsync();

        }
        public async Task<SubjectMaster> UpdateAsync(SubjectMaster entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.SubjectMaster.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();

        }
    }
}
