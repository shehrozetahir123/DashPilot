using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyApplication.Application.Responses
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = true;
            StatusCode = HttpStatusCode.OK;
        }
        public BaseResponse(string message)
        {
            Success = true;
            Message = message;
            StatusCode = HttpStatusCode.OK;
        }

        public BaseResponse(string message, bool success)
        {
            Success = success;
            Message = message;
            StatusCode = HttpStatusCode.OK;
        }

        public bool Success { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public List<string>? ValidationErrors { get; set; }

        public object objectresponse { get; set; }
    }
}
