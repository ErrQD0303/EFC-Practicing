using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC_Models;

namespace EFC_Interfaces
{
    public interface IDBActions
    {
        Task CreateDatabase();
        Task DeleteDatabase();
        Task Insert(params IEnumerable<Product> products);
        Task SelectAll();
        Task SelectById(int id);
    }
}