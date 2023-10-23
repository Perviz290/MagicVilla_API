using System.Net;

namespace MagicVilla_VillaAPI.Model
{
    public class APIResponse
    {

        public HttpStatusCode statusCode {  get; set; }
        public bool InSuccess { get; set; } = true;  
        public List<string> ErrorMessages{ get; set; }
        public object Result { get; set; }


    }
}
