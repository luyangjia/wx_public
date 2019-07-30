using System;
using System.Collections.Generic;
using System.Web;

namespace WxPay2017.API.WEBAPI
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}