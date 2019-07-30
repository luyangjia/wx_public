using System;
using System.Collections.Generic;
using System.Net;
namespace WxPay2017.API.VO
{
    public partial class ConfirmMessageData
    {
        public ConfirmMessageData()
        {
            Status = HttpStatusCode.OK;
            Messages = new List<string>();
        }
        public HttpStatusCode Status { get; set; }

        public IEnumerable<string> Messages { get; set; } 

        public Exception Exception { get; set; }
    }
}
