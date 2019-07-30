using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPay2017.API.BLL;
using WxPay2017.API.VO;

namespace WxPay2017.API.WEBAPI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
           // BalanceBLL balancebll = new BalanceBLL();
           // HistoryBillData billdata = balancebll.GetPayOrder(12, "owDsuwF-DB_0UXfM_DEhbNJYOjug");

            
            //balancebll.PayOk("148665940220170816150240580634").ConfigureAwait(true);
           // balancebll.PayOk("80649").ConfigureAwait(true);


            return View();
        }
    }
}