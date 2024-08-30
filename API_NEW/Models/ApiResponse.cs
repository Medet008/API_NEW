using System.Net;

namespace API_NEW.Models
{
    public class ApiResponse
    {
        public ApiResponse() 
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<String> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
