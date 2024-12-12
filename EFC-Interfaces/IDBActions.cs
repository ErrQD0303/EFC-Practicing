using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFC_Interfaces
{
    public interface IDBActions
    {
        Task CreateDatabase();
        Task DeleteDatabase();
        Task AddAsync<TEntity>(params IEnumerable<TEntity> collections) where TEntity : class, IEFModel;
        Task<IEnumerable<TEntity>?> GetAllAsync<TEntity>() where TEntity : class, IEFModel;
        Task<TEntity?> GetByIdAsync<TEntity>(int id) where TEntity : class, IEFModel;
        Task<IEnumerable<TEntity>?> GetByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEFModel;
        Task<TEntity?> UpdateAsync<TEntity>(int id, TEntity record) where TEntity : class, IEFModel;
        Task DeleteAsync<TEntity>(int id) where TEntity : class, IEFModel;
    }
}