using WxPay2017.API.WEBAPI.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;


namespace WxPay2017.API.WEBAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}", 
                defaults: new { id = RouteParameter.Optional }
            );

         
            var formatter = config.Formatters;
            formatter.Remove(config.Formatters.XmlFormatter); 
            var json = formatter.JsonFormatter;
            // 解决json序列化时的循环引用问题
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //对 JSON 的日期数据进行格式化
            var dateTimeConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            json.SerializerSettings.Converters.Add(dateTimeConverter);
            // 对 JSON 数据使用混合大小写。驼峰式,但是是javascript 首字母小写形式.
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        

            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json-patch+json"));

            //config.MessageHandlers.Add(new OptionsHttpMessageHandler(config));

            // 异常过滤
            config.Filters.Add(new Core.CustomExceptionFilterAttribute());
        }
    }
}
