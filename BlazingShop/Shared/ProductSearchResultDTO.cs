using System.Collections.Generic;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Shared
{
    public class ProductSearchResultDTO
    {
        public List<Product> Products { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }

    }
}
