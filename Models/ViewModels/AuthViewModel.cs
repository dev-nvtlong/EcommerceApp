using EcommerceApp.Application.DTOs.Account;

namespace EcommerceApp.Models.ViewModels
{
    public class AuthViewModel
    {
        public LoginDto LoginModel { get; set; } = new();
        public RegisterDto RegisterModel { get; set; } = new();
        public bool IsRegisterActive { get; set; }
    }
}
