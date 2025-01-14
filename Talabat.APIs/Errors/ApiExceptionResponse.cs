namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }

        public ApiExceptionResponse(int satausCode,string? message=null , string? details = null)
            : base(satausCode,message )
        {
            Details = details;
        }
    }
}
