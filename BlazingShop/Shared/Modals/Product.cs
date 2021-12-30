using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingShop.Shared.Modals
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Featured { get; set; } = false;
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
