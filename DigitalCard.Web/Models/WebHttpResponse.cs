using System.Net;

namespace DigitalCard.Web.Models
{
    public class WebHttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Data { get; set; }
    }
}
