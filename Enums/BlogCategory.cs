using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Enums
{
    public enum BlogCategory
    {
        [Display(Name = "Bài viết sản phẩm")]
        Sales = 1,      // Bài viết bán hàng

        [Display(Name = "Kinh nghiệm chăm sóc")]
        Experience = 2, // Kinh nghiệm chăm sóc
    }
}
