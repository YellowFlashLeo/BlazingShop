using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }
        Task GetCategories();
    }
}
