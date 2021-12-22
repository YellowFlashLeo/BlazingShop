using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Shared;
using BlazingShop.Shared.Modals;
using Microsoft.EntityFrameworkCore;

namespace BlazingShop.Server.DataBase.Operations.CategoryServiceDB
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;
        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var categories = await _dataContext.Categories.ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }
    }
}
