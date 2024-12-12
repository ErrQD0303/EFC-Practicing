using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EFC_Attributes;

namespace EFC_Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository Repository { get; }

        [RepositoryTable("Category")]
        ICategoryRepository Categories { get; }

        [RepositoryTable("Product")]
        IProductRepository Products { get; }
        Task SaveChangesAsync();
    }
}