using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Shared;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Client.Services.ProductService
{
   public interface IProductService
   {
        event Action ProductsChanged;
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        List<Product> Products { get; set; }
        Task GetProducts(string categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductById(int id);
        Task SearchProducts(string searchText, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchText);

   }
}
