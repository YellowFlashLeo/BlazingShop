namespace BlazingShop.Shared.DTOs
{
   public class CartItemDTO
    {
        public int ProductId { get; set; }
        public int EditionId { get; set; }
        public string Title { get; set; }
        public string EditionName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
    }
}
