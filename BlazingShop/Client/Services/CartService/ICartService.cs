using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingShop.Shared;
using BlazingShop.Shared.DTOs;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Client.Services.CartService
{
  public interface ICartService
  {
      event Action OnChange;
      Task AddToCart(ProductVariant productVariant);
      Task<List<CartItemDTO>> GetCartItems();
      Task DeleteItem(CartItemDTO cartItem);
  }
}
