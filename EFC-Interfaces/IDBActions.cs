using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Helpers;

namespace EFC_Interfaces
{
    public interface IDBActions
    {
        Task CreateDatabase();
        Task DeleteDatabase();
        Task AddAsync<TEntity>(params IEnumerable<TEntity> collections) where TEntity : class, IEFModel;
        Task<IEnumerable<TEntity>?> GetAllAsync<TEntity>() where TEntity : class, IEFModel;
        Task<TEntity?> GetByIdAsync<TEntity>(int id) where TEntity : class, IEFModel;
        Task<IEnumerable<TEntity>?> GetByConditionAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate) where TEntity : class, IEFModel, new();
        Task<IEnumerable<object>?> JoinAndGetByConditionAsync<TEntity, TJoinEntity>(Expression<Func<TEntity, int?>> outerKeySelector, Expression<Func<TJoinEntity, int?>> innerKeySelector, JoinType joinType, Expression<Func<TEntity, bool>>? outerPredicate = null, Expression<Func<TJoinEntity, bool>>? innerPredicate = null, Expression<Func<TEntity, object>>? outerOrderByKeySelector = null, bool outerOrderByAscending = true, Expression<Func<TJoinEntity, object>>? innerOrderByKeySelector = null, bool innerOrderByAscending = true, int? take = null, int? skip = null, IEnumerable<string>? outerGetFields = null, IEnumerable<string>? innerGetFields = null)
        where TEntity : class, IEFModel, new()
        where TJoinEntity : class, IEFModel, new();
        Task<TEntity?> UpdateAsync<TEntity>(int id, TEntity record) where TEntity : class, IEFModel;
        Task DeleteAsync<TEntity>(int id) where TEntity : class, IEFModel;
        Task<IEnumerable<TEntity>?> FromSQLRawAsync<TEntity>(string? sql) where TEntity : class;
    }
}