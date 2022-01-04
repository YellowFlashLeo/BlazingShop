using System.Collections.Generic;
using BlazingShop.Shared.DTOs;
using Stripe.Checkout;

namespace BlazingShop.Server.DataBase.Operations.PaymentService
{
    public interface IPaymentService
    {
        Session CreateCheckoutSession(List<CartItemDTO> cartItems);
    }
}
