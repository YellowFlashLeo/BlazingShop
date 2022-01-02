using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Client.Services.ProductService;
using BlazingShop.Shared.Modals;
using Blazored.LocalStorage;
using Blazored.Toast.Services;

namespace BlazingShop.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IToastService _toastService;
        private readonly IProductService _productService;

        public event Action OnChange;

        public CartService(
            ILocalStorageService localStorage,
            IToastService toastService,
            IProductService productService)
        {
            _localStorage = localStorage;
            _toastService = toastService;
            _productService = productService;
        }
        
        public  async Task AddToCart(ProductVariant productVariant)
        {
            var cart = await _localStorage.GetItemAsync<List<ProductVariant>>("cart") ?? new List<ProductVariant>();

            cart.Add(productVariant);
            await _localStorage.SetItemAsync("cart", cart);

            var product = await _productService.GetProductById(productVariant.ProductId);
            _toastService.ShowSuccess(product.Data.Title,"Added to cart:");

            OnChange.Invoke();
        }
    }
}
