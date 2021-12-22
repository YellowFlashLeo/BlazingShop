using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Shared;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Server.DataBase.Operations.CategoryServiceDB
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategories();
    }
}
