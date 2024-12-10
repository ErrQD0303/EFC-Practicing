using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFC_Interfaces
{
    public interface IDBActions<T>
    where T : class, IEFModel
    {
        Task CreateDatabase();
        Task DeleteDatabase();
        Task<bool> Insert(params IEnumerable<T> collections);
        Task<IEnumerable<T>?> SelectAll();
        Task<T?> SelectById(int id);
        Task<T?> Update(int id, T record);
        Task<bool> Delete(int id);
    }
}