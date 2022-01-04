using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazingShop.Client.Services.ProductService;
using BlazingShop.Shared.DTOs;
using Blazored.LocalStorage;
using Blazored.Toast.Services;

namespace BlazingShop.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IToastService _toastService;
        private readonly IProductService _productService;
        private readonly HttpClient _httpClient;

        public event Action OnChange;

        public CartService(
            ILocalStorageService localStorage,
            IToastService toastService,
            IProductService productService,
            HttpClient httpClient)
        {
            _localStorage = localStorage;
            _toastService = toastService;
            _productService = productService;
            _httpClient = httpClient;
        }
        
        public  async Task AddToCart(CartItemDTO cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItemDTO>>("cart") ?? new List<CartItemDTO>();

            var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId && x.EditionId == cartItem.EditionId);
            if (sameItem == null)
            {
                cart.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }
           
            await _localStorage.SetItemAsync("cart", cart);

            var product = await _productService.GetProductById(cartItem.ProductId);
            _toastService.ShowSuccess(product.Data.Title,"Added to cart:");

            OnChange.Invoke();
        }

        public async Task<List<CartItemDTO>> GetCartItems()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItemDTO>>("cart");
            if (cart == null)
                return new List<CartItemDTO>();

            return cart;
        }

        public async Task DeleteItem(CartItemDTO cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItemDTO>>("cart");
            if (cart == null)
                return;

            var item = cart.Find(x => x.ProductId == cartItem.ProductId && x.EditionId == cartItem.EditionId);
            cart.Remove(item);

            await _localStorage.SetItemAsync("cart", cart);
            _toastService.ShowError(cartItem.Title, "Removed from the cart");
            OnChange.Invoke();
        }

        public async Task EmptyCart()
        {
            await _localStorage.RemoveItemAsync("cart");
            OnChange.Invoke();
        }

        public async Task<string> Checkout()
        {
            var result = await _httpClient.PostAsJsonAsync("api/payment/checkout", await GetCartItems());
            var url = await result.Content.ReadAsStringAsync();
            return url;
        }
    }
}
