using System.Linq.Expressions;
using EFC_Attributes;
using EFC_Interfaces;
using EFC_Models;
using EFC_Repositories;
using EFC_Repositories.Interfaces;
using Helpers;

namespace EFC_DBActions;

public class DBShopActions : IDBActions
{
    IUnitOfWork _unitOfWork;

    public DBShopActions(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateDatabase()
    {
        await _unitOfWork.Repository.CreateDatabase();
    }

    public async Task DeleteDatabase()
    {
        await _unitOfWork.Repository.DeleteDatabase();
    }

    public async Task AddAsync<TEntity>(params IEnumerable<TEntity> records) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        await entityRepository.AddAsync(records.ToArray());
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync<TEntity>() where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        return await entityRepository.GetAll();
    }

    public async Task<TEntity?> GetByIdAsync<TEntity>(int id) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        return await entityRepository.GetByIdAsync(id);
    }
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await GetByIdAsync<Product>(id);
    }
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await GetByIdAsync<Category>(id);
    }

    public async Task<IEnumerable<TEntity>?> GetByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        return await entityRepository.FindAsync(predicate);
    }

    public async Task<IEnumerable<Product>?> GetProductByName(string name)
    {
        return await _unitOfWork.Products.FindAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<Product>?> GetProductByProvider(string provider)
    {
        return await _unitOfWork.Products.FindAsync(p => p.Provider == provider);
    }

    public async Task<TEntity?> UpdateAsync<TEntity>(int id, TEntity record) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        await entityRepository.UpdateAsync(id, record);
        await _unitOfWork.SaveChangesAsync();
        System.Console.WriteLine($"{typeof(TEntity).Name} updated");

        var returnRecord = await entityRepository.GetByIdAsync(id);
        new List<TEntity?>() { returnRecord }.ShowItems();

        return returnRecord;
    }
    public async Task<Product?> UpdateName(int id, string name)
    {
        var product = await GetByIdAsync<Product>(id);
        if (product is null)
        {
            System.Console.WriteLine("Product not found");
            return null;
        }
        return await UpdateAsync(id, new Product { Name = name, CategoryId = product.CategoryId });
    }
    public async Task<Product?> UpdateProvider(int id, string provider)
    {
        var product = await GetByIdAsync<Product>(id);
        if (product is null)
        {
            System.Console.WriteLine("Product not found");
            return null;
        }
        return await UpdateAsync(id, new Product { Provider = provider, CategoryId = product.CategoryId });
    }

    public async Task DeleteAsync<TEntity>(int id) where TEntity : class, IEFModel
    {
        var entityRepository = _unitOfWork.GetUnitOfWorkProperty<IUnitOfWork, RepositoryTableAttribute, TEntity>() as IRepository<TEntity> ?? throw new NullReferenceException();
        await entityRepository.DeleteAsync(id);
        await entityRepository.SaveChangesAsync();
    }
}
