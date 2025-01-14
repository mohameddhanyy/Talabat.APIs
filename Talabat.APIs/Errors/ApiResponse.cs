
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode , string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetErrorMessageByCode(statusCode);
        }

        private string? GetErrorMessageByCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "a Bad Request :( ",
                401 => "You're Unauthorized :(",
                404 => "a Resource Not Found",
                500 => "We Have a Problem right now ",
                _ => null,
            };
        }
    }
}
