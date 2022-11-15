using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIgitalCard.Lib.Models
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
