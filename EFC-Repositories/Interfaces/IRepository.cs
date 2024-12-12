using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFC_Repositories.Interfaces
{
    public interface IRepository
    {
        public IUnitOfWork UnitOfWork { get; }
        Task CreateDatabase();
        Task DeleteDatabase();
        Task SaveChangesAsync();
    }

    public interface IRepository<T> : IRepository
    where T : class
    {
        Task<IEnumerable<T>?> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(params T[] records);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> predicate);
    }
}