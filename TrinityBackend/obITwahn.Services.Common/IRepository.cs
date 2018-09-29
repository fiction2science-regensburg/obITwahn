using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace obITwahn.Services.Common
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(Guid? id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> filter);

        Task SaveOrUpdateAsync(T entity);

        Task DeleteAsync(Guid? id);
    }
}