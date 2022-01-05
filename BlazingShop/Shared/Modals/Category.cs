using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingShop.Shared.Modals
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string  Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
}
