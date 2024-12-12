using EFC_ModelContext;
using EFC_Models;
using EFC_Repositories.Interfaces;

namespace EFC_Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWork, params ShopContext[] contexts) : base(unitOfWork, contexts)
        {
        }
    }
}