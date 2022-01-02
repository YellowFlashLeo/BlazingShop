using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Client.Services.ProductService;
using BlazingShop.Shared;
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

        public async Task<List<CartItemDTO>> GetCartItems()
        {
            var result = new List<CartItemDTO>();
            var cart = await _localStorage.GetItemAsync<List<ProductVariant>>("cart");
            if (cart == null)
                return result;

            foreach (var cartItem in cart)
            {
                var product = await _productService.GetProductById(cartItem.ProductId);
                var resultingCartItem = new CartItemDTO()
                {
                    ProductId = product.Data.Id,
                    Title = product.Data.Title,
                    Image = product.Data.ImageUrl
                };
                var variant = product.Data.Variants.Find(v => v.ProductTypeId == cartItem.ProductTypeId);
                if (variant != null)
                {
                    resultingCartItem.EditionId = variant.ProductTypeId;
                    resultingCartItem.EditionName = variant.ProductType.Name;
                    resultingCartItem.Price = variant.Price;
                }

                result.Add(resultingCartItem);
            }

            return result;
        }

        public async Task DeleteItem(CartItemDTO cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<ProductVariant>>("cart");
            if (cart == null)
                return;

            var item = cart.Find(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.EditionId);
            cart.Remove(item);

            await _localStorage.SetItemAsync("cart", cart);
            _toastService.ShowError(cartItem.Title, "Removed from the cart");
            OnChange.Invoke();
        }
    }
}
