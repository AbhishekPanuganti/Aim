using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMSAPI
{
    public class AntiXssMiddleware
    {
    }

    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }
    }
}