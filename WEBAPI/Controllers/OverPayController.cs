using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPay2017.API.BLL;
using WxPay2017.API.VO;
namespace WxPay2017.API.WEBAPI.Controllers
{
    public class OverPayController : Controller
    {
        // GET: OverPay
        public ActionResult Index()
        {
            string strno=Request["strno"];
            BalanceBLL balancebll = new BalanceBLL();
            // HistoryBillData billdata = balancebll.GetPayOrder(12, "owDsuwF-DB_0UXfM_DEhbNJYOjug");


            balancebll.PayOk(strno).ConfigureAwait(true);


            return View();
        }
    }
}