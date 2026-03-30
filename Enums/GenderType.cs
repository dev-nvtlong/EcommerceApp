using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Enums
{
    public enum GenderType
    {
        [Display(Name = "Chưa xác định")]
        Unknown = 0,
        [Display(Name = "Nam")]
        Male = 1,
        [Display(Name = "Nữ")]
        Female = 2,
        [Display(Name = "Khác")]
        Other = 3,
    }
}
