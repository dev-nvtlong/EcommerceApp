using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Enums
{
    public enum PaymentMethod
    {
        [Display(Name = "Tiền mặt")]
        Cash = 0,  

        [Display(Name = "Chuyển khoản")]
        Banking = 1    
    }
}
