using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository
{
    public class CountryMasterRepository : ICountryMasterRepository 
    {
        private readonly ApplicationDBContext _db;

        public CountryMasterRepository(ApplicationDBContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(CountryMaster entity, int userId)
        {

            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            await _db.AddAsync(entity);
            await SaveAsync();
        }


        public async Task<List<CountryMaster>> GetAllAsync(Expression<Func<CountryMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<CountryMaster> query = _db.CountryMaster;
            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
        //"Villa,VillaSpecial"
        public async Task<CountryMaster> GetAsync(Expression<Func<CountryMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<CountryMaster> query = _db.CountryMaster;
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

        public async Task RemoveAsync(CountryMaster entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.CountryMaster.Update(entity);
            await SaveAsync();

        }
        public async Task<CountryMaster> UpdateAsync(CountryMaster entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.CountryMaster.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public bool IsUniqueName(string CountryName, int CountryId)
        {
            var user = _db.CountryMaster.FirstOrDefault(x => x.CountryName == CountryName&&x.CountryId!=CountryId);
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
