namespace EcommerceApp.Application.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; } = string.Empty;
        public object? Error { get; set; }
        public static ApiResponse Ok(string message)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        public static ApiResponse Fail(string message, object? error = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Error = error
            };
        }
    }
}
