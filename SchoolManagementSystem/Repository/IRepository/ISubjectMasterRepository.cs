using SchoolManagementSystem.Models;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Repository.IRepository
{
    public interface ISubjectMasterRepository
    {
        Task<List<SubjectMaster>> GetAllAsync(Expression<Func<SubjectMaster, bool>> filter = null, bool tracked = true);
        Task<SubjectMaster> GetAsync(Expression<Func<SubjectMaster, bool>> filter = null, bool tracked = true);
        Task CreateAsync(SubjectMaster entity,int userId);
        Task RemoveAsync(SubjectMaster entity, int userId);
        Task<SubjectMaster> UpdateAsync(SubjectMaster entiy,int userId);
        Task SaveAsync();
    }
}
