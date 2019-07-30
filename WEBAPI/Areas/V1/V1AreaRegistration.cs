using System.Web.Mvc;

namespace WxPay2017.API.WEBAPI.Areas.V1
{
    public class V1AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "V1";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "V1_default",
                "V1/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}