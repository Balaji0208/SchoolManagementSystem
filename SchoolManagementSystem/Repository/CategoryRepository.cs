using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repository.IRepository;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _db;

        public CategoryRepository(ApplicationDBContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(Categories entity, int userId)
        {

            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            await _db.AddAsync(entity);
            await SaveAsync();
        }


        public async Task<List<Categories>> GetAllAsync(Expression<Func<Categories, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Categories> query = _db.Categories;
            if (filter != null)
            {
                query = query.Where(filter);

            }


            return await query.ToListAsync();
        }
        //"Villa,VillaSpecial"
        public async Task<Categories> GetAsync(Expression<Func<Categories, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Categories> query = _db.Categories;
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

        public async Task RemoveAsync(Categories entity, int userId)
        {

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.Categories.Update(entity);
            await SaveAsync();

        }
        public async Task<Categories> UpdateAsync(Categories entity, int userId)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public bool IsUniqueName(string CategoryName,int CategoryId)
        {
            var user = _db.Categories.FirstOrDefault(x => x.CategoryName == CategoryName&&x.CategoryId!=CategoryId);
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
