using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace WxPay2017.API.WEBAPI.Core
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                //Content = new StringContent(Jil.JSON.Serialize(this.BuildInnerException(context.Exception))),
                Content = new StringContent(Jil.JSON.Serialize(this.BuildInnerException(context.Exception)), Encoding.GetEncoding("UTF-8"), "application/json"),
                ReasonPhrase = "Internal Server Error",
            };
            //if (context.Exception is ArgumentException || context.Exception is IndexOutOfRangeException)
            //{
            //}
        }

        private ExceptionMessager BuildInnerException(Exception inner)
        {
            ExceptionMessager messager = new ExceptionMessager();
            if (inner != null)
            {
                messager.ErrorMessage = inner.Message;
                messager.Source = inner.Source;
                messager.StackTrace = inner.StackTrace;
                messager.DataCount = inner.Data.Count;
            }
            if (inner.InnerException != null)
            {
                messager = this.BuildInnerException(inner.InnerException);
            }
            return messager;
        }
    }

    public class ExceptionMessager
    {
        public string ErrorMessage { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public int DataCount { get; set; }

        public string InnerExecption { get; set; }
    }
}