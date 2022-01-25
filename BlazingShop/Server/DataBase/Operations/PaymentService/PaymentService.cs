using System;
using System.Collections.Generic;
using BlazingShop.Shared.DTOs;
using Stripe;
using Stripe.Checkout;

namespace BlazingShop.Server.DataBase.Operations.PaymentService
{
    public class PaymentService :IPaymentService
    {
        public PaymentService()
        {
            StripeConfiguration.ApiKey = "sk_test_51KDphzGM2RtQSycGQTQhHjnUFq39vzlluih1vDPzYiJ2AS960me4X783law4qLWwOLiMXxdF9zzUl7Q94Kv0xnDl00UtyKWD4Q";
        }
        public Session CreateCheckoutSession(List<CartItemDTO> cartItems)
        {
            var lineItems = new List<SessionLineItemOptions>();
            cartItems.ForEach(ci => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions 
                {
                    UnitAmountDecimal = ci.Price *100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = ci.Title,
                        Images = new List<string>{ci.Image}
                    }
                },
                Quantity = ci.Quantity
            }));
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {"card"},
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:44376/order-success",
                CancelUrl = "https://localhost:44376/cart",
            };
            var service = new SessionService();
            Session session = new Session();
            session = service.Create(options);

            return session;
        }

    }
}
