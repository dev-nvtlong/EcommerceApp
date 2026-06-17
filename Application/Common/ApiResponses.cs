namespace EcommerceApp.Application.Common
{
    public class ApiResponses<T> : ApiResponse
    {
        public T? Data { get; set; }
        public static ApiResponses<T> Ok(T data, string message = "Success")
        {
            return new ApiResponses<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
        public new static ApiResponses<T> Fail(string message, object? error = null)
        {
            return new ApiResponses<T>
            {
                Success = false,
                Message = message,
                Error = error
            };
        }
    }
}
