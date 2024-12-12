using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC_Interfaces;
using EFC_Models;

namespace EFC_Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    }
}