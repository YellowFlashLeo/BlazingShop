using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Shared;
using BlazingShop.Shared.DTOs;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Server.DataBase.Operations.ProductServiceDB
{
   public interface IProductService
   {
       Task DeleteProduct(int id);
       Task AddProduct(Product product);
       Task<ServiceResponse<List<Product>>> GetProducts();
       Task<ServiceResponse<Product>> GetSingleProduct(int id);
       Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
       Task<ServiceResponse<ProductSearchResultDTO>> SearchProducts(string searchText, int page);
       Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
       Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
   }
}
