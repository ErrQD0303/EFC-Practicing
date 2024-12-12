using EFC_Interfaces;
using EFC_ModelContext;
using EFC_Repositories.Interfaces;

namespace EFC_Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(params IEFDBConnection[] efConnections)
        {
            var shopContext = efConnections.Select(c => new ShopContext(c)).ToArray();
            Repository = new Repository(this, shopContext);
            Categories = new CategoryRepository(this, shopContext);
            Products = new ProductRepository(this, shopContext);
        }

        public IRepository Repository { get; }

        public ICategoryRepository Categories { get; }

        public IProductRepository Products { get; }


        public async Task SaveChangesAsync()
        {
            await Repository.SaveChangesAsync();
        }
    }
}