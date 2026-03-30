using EcommerceApp.Base;

namespace EcommerceApp.Models
{
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }

}
