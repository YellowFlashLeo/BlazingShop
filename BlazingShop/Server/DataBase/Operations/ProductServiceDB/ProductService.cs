using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingShop.Shared;
using BlazingShop.Shared.Modals;
using Microsoft.EntityFrameworkCore;

namespace BlazingShop.Server.DataBase.Operations.ProductServiceDB
{
    public class ProductService : IProductService
    {
        private readonly DataContext _dataContext;

        public ProductService(DataContext databaseContext)
        {
            _dataContext = databaseContext;
        }

        public async Task DeleteProduct(int id)
        {
            var product = _dataContext.Products
                .FirstOrDefault(h => h.Id == id);
            if (product != null)
            {
                _dataContext.Products.Remove(product);
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Requested product doesn't exist");
            }
        }

        public async Task AddProduct(Product product)
        {
            if (product != null)
            {
                _dataContext.Products.Add(product);
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Product to be added is null!");
            }
        }

        public async Task<ServiceResponse<Product>> GetSingleProduct(int id)
        {
            var response = new ServiceResponse<Product>();
            var productFromDb = await _dataContext.Products
                .Include( p=>p.Variants)
                .ThenInclude(p =>p.ProductType)
                .FirstOrDefaultAsync(p=>p.Id == id);

            if (productFromDb == null)
            {
                response.Success = false;
                response.Message = "Sorry, requested product does not exist.";
            }
            else
            {
                response.Data = productFromDb;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _dataContext.Products
                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                    // cause we need access to corresponding price
                    .Include(p => p.Variants)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> SearchProducts(string searchText)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await FindProductsBySearchText(searchText)
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);

            List<string> suggestions = new List<string>();
            foreach (var product in products)
            {                                          // .ToLower()
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(product.Title);
                }

                if (product.Description != null)
                {
                    // we get all possible marks like . , : from description of the product
                    var punctuation = product.Description.Where(char.IsPunctuation)
                                                               .Distinct()
                                                               .ToArray();
                    // we get IEnumerable of words without punctuation marks
                    var words = product.Description.Split()
                                                                  .Select(w => w.Trim(punctuation));
                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) &&
                            !suggestions.Contains(word))
                        {
                            suggestions.Add(word);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>>
            {
                Data = suggestions
            };
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _dataContext.Products
                    .Where(p => p.Featured)
                    .Include(p => p.Variants)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProducts()
        {
            var products = await _dataContext.Products
                .Include(p => p.Variants)
                .ThenInclude(p => p.ProductType)
                .ToListAsync();
            var response = new ServiceResponse<List<Product>>
            {
                Data = products
            };

            return response;

        }
        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _dataContext.Products
                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) 
                || p.Description.ToLower().Contains(searchText.ToLower()))
                .Include(p => p.Variants)
                .ThenInclude(p => p.ProductType)
                .ToListAsync();
        }
        //public async Task UpdateProduct(Product product, int id)
        //{
        //    var productDb = _dataContext.Products 
        //        .FirstOrDefaultAsync(h => h.Id == id);
        //    if (productDb != null)
        //    {
        //        productDb.Title = product.Title;
        //        productDb.Description = product.Description;
        //        productDb.Price = product.Price;
        //        productDb.ImageUrl = product.ImageUrl;
        //    }

        //    else
        //    {
        //        throw new InvalidOperationException("Requested product doesn't exist");
        //    }

        //    _dataContext.Update(productDb);
        //    await _dataContext.SaveChangesAsync();
        //}
    }
}
