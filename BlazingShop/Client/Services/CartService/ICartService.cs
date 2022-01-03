using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Shared.DTOs;

namespace BlazingShop.Client.Services.CartService
{
  public interface ICartService
  {
      event Action OnChange;
      Task AddToCart(CartItemDTO cartItem);
      Task<List<CartItemDTO>> GetCartItems();
      Task DeleteItem(CartItemDTO cartItem);
      Task EmptyCart();
  }
}
