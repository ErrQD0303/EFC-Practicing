using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC_Interfaces;
using EFC_ModelContext;
using EFC_Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFC_Repositories
{
    public class Repository : IRepository
    {
        protected readonly ShopContext[] _shopContexts;
        public IUnitOfWork UnitOfWork { get; }

        public Repository(IUnitOfWork unitOfWork, params ShopContext[] shopContexts)
        {
            _shopContexts = shopContexts;
            UnitOfWork = unitOfWork;
        }

        public async Task SaveChangesAsync()
        {
            foreach (ShopContext context in _shopContexts)
            {
                await context.SaveChangesAsync();
            }
        }

        async Task IRepository.CreateDatabase()
        {
            foreach (ShopContext context in _shopContexts)
            {
                var databaseName = context.Database.GetDbConnection().Database;
                System.Console.WriteLine("Create " + databaseName);
                bool result = await context.Database.EnsureCreatedAsync();
                string resultString = result ? "created" : "already exists";
                System.Console.WriteLine($"Database {databaseName} {resultString}");
            }
        }

        async Task IRepository.DeleteDatabase()
        {
            var databaseName = _shopContexts[0].Database.GetDbConnection().Database;
            System.Console.WriteLine($"Do you want to delete {databaseName}? (y)");
            string? input = Console.ReadLine();
            if (String.Equals(input, "y", StringComparison.OrdinalIgnoreCase))
            {
                foreach (ShopContext context in _shopContexts)
                {
                    var deleted = await context.Database.EnsureDeletedAsync();
                    var deletionInfo = deleted ? "deleted" : "not deleted";
                    System.Console.WriteLine($"{databaseName} {deletionInfo}");
                }
            }
        }

        async Task<IEnumerable<T>?> IRepository.FromSqlRaw<T>(string? sql)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            var context = _shopContexts[0];

            var result = await context.Set<T>().FromSqlRaw(sql).ToListAsync();
            return result;
        }
    }
}