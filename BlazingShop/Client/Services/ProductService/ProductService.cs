using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazingShop.Shared;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public event Action ProductsChanged;
        public string Message { get; set; } = "Loading products...";
        public List<Product> Products { get; set; } = new List<Product>();
        public async Task GetProducts(string categoryUrl = null)
        {
            var result = categoryUrl == null
                ? await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/featured")
                : await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/Category/{categoryUrl}");
            Products = result?.Data;
            ProductsChanged.Invoke();
        }

        public async Task<ServiceResponse<Product>> GetProductById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{id}");
            return result;
        }

        public async Task SearchProducts(string searchText)
        {
            var result = await _httpClient
                .GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");
            Products = result.Data;
            if (Products.Count == 0)
            {
                Message = "No products found";
            }
            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _httpClient
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");

            return result?.Data;
        }
    }
}
