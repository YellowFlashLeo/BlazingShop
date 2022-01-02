using System;
using System.Threading.Tasks;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Client.Services.CartService
{
  public interface ICartService
  {
      event Action OnChange;
      Task AddToCart(ProductVariant productVariant);
  }
}
