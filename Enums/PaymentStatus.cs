using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "Đang chờ xử lý")]
        Pending = 0,

        [Display(Name = "Đã thanh toán")]
        Paid = 1,

        [Display(Name = "Thất bại")]
        Failed = 2
    }
}
