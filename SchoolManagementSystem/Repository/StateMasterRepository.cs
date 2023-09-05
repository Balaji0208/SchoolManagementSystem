using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository
{
    public class StateMasterRepository : IStateMasterRepository 
    {
        private readonly ApplicationDBContext _db;

        public StateMasterRepository(ApplicationDBContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(StateMasterDTO entity, int userId)
        {
            StateMaster state = new();
            state.CountryId = entity.CountryId;
            state.CreatedBy = userId;
            state.UpdatedBy = userId;
            state.CreatedDate = DateTime.Now;
            state.UpdatedDate = DateTime.Now;
            state.StateCode= entity.StateCode;
            state.StateName= entity.StateName;


            await _db.AddAsync(state);
            await SaveAsync();
        
            
        }


        public async Task<List<StateMaster>> GetAllAsync(Expression<Func<StateMaster, bool>> filter = null, bool tracked = true, string? includeProperties=null)
        {
            IQueryable<StateMaster> query = _db.StateMaster;

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
        //"Villa,VillaSpecial"
        public async Task<StateMaster> GetAsync(Expression<Func<StateMaster, bool>> filter = null, bool tracked = true)
        {
            IQueryable<StateMaster> query = _db.StateMaster;
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

        public async Task RemoveAsync(StateMaster entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.StateMaster.Update(entity);
            await SaveAsync();

        }
        public async Task<StateMaster> UpdateAsync(StateMaster entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.StateMaster.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public bool IsUniqueName(string StateName, int StateId)
        {
            var user = _db.StateMaster.FirstOrDefault(x => x.StateName == StateName && x.StateId!=StateId);
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
