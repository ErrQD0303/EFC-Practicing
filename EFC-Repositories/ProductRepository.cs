using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFC_ModelContext;
using EFC_Models;
using EFC_Repositories.Interfaces;

namespace EFC_Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWork, params ShopContext[] contexts) : base(unitOfWork, contexts)
        {
        }

        public Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}