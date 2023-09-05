using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository
{
    public class DistrictMasterRepository : IDistrictMasterRepository
    {
        private readonly ApplicationDBContext _db;

        public DistrictMasterRepository(ApplicationDBContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(DistrictMaster entity, int userId)
        {

            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            await _db.AddAsync(entity);
            await SaveAsync();
        }


        public async Task<List<DistrictMaster>> GetAllAsync(Expression<Func<DistrictMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<DistrictMaster> query = _db.DistrictMaster;
            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
        //"Villa,VillaSpecial"
        public async Task<DistrictMaster> GetAsync(Expression<Func<DistrictMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<DistrictMaster> query = _db.DistrictMaster;
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

        public async Task RemoveAsync(DistrictMaster entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.DistrictMaster.Update(entity);
            await SaveAsync();

        }
        public async Task<DistrictMaster> UpdateAsync(DistrictMaster entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.DistrictMaster.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public bool IsUniqueName(string DistrictName)
        {
            var user = _db.DistrictMaster.FirstOrDefault(x => x.DistrictName == DistrictName);
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
