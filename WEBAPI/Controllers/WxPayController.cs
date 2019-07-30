using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPay2017.API.BLL;
using WxPay2017.API.VO;

namespace WxPay2017.API.WEBAPI.Controllers
{
    public class WxPayController : Controller
    {
        // GET: WxPay 下单支付

        public static string wxJsApiParam { get; set; } //H5调起JS API参数
   
        public ActionResult Index(string amountid, string hidopenid)
        {

            BalanceBLL balancebll = new BalanceBLL();
            Log.Debug(this.GetType().ToString(), "amount: " + amountid);
            Log.Debug(this.GetType().ToString(), "openid: " + hidopenid);
            ViewBag.nowdata = DateTime.Now.ToString();
           // string out_trade_no = WxPayApi.GenerateOutTradeNo();
            string out_trade_no = "";

            JsApiPay jsApiPay = new JsApiPay(System.Web.HttpContext.Current);
            //JSAPI支付预处理
            try
            {
                ViewBag.Amount = amountid;
         
                double  value = double.Parse(amountid);

                decimal dvalue = decimal.Parse(amountid);
           
                int payamount = int.Parse((value * 100).ToString());
                Log.Debug(this.GetType().ToString(), "payamount : " + payamount);

                HistoryBillData billdata = balancebll.GetPayOrder(dvalue, hidopenid);

                out_trade_no = billdata.Id.ToString();
                out_trade_no = WxPayApi.GenerateOutTradeNo() + out_trade_no;

                ViewBag.orderno = billdata.Id.ToString();
                //Log.Debug(this.GetType().ToString(), "out_trade_no : " + out_trade_no);
                //Log.Debug(this.GetType().ToString(), "out_trade_no23 : " + out_trade_no.Substring(25));
           
              
                //这里先写入数据库，写好了在发起API的调用付款
              
               
                //result=WriteData(out_trade_no,hidopenid,amountid);


                if (!string.IsNullOrEmpty(out_trade_no))
                {
                    //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数 
                    jsApiPay.openid = hidopenid;
                    jsApiPay.total_fee = payamount;// 订单金额(1表示分,正式金额要*100)

                    ViewBag.out_trade_no = out_trade_no;
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(out_trade_no); 
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数   
                ViewBag.wxJsApiParam = wxJsApiParam;   //前台页面js调用 
                
              
                //在页面上显示订单信息 
                }
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), "ex : " + ex.Message);
            }



            return View();





        }



 


    }
}