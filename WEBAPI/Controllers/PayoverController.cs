using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPay2017.API.BLL;

namespace WxPay2017.API.WEBAPI.Controllers
{
    public class PayoverController : Controller
    {
        // GET: Payover
        public ActionResult Index()
        {
            Log.Debug("收到通知:", "收到通知");
            //微信会把付款结果通知到这里 
            ResultNotify resultNotify = new ResultNotify(System.Web.HttpContext.Current);
            WxPayData notifyData = resultNotify.GetNotifyData();
            //序列化  notifyData
            NotifyModel notify = new NotifyModel();
            notify = JSONHelper.JsonDeserialize<NotifyModel>(notifyData.ToJson());
               
            //////////这个必须给微信的通知回复收到信息,不然微信会一直发8次 ///////
            WxPayData res = new WxPayData();
            res.SetValue("return_code", "SUCCESS");
            res.SetValue("return_msg", "OK");
            HttpContext.Response.Clear();
            HttpContext.Response.Write(res.ToXml());
            HttpContext.Response.End();
            //////////回复结束////////////// 
            Log.Debug("通知结束:", "通知结束");
          
            if (notify.result_code == "SUCCESS" && notify.return_code == "SUCCESS" && !string.IsNullOrEmpty(notify.out_trade_no) && !string.IsNullOrEmpty(notify.transaction_id))
            {
                //根据后台的充值记录
               // Log.Debug("微信支付结果通知:", "序列化后的openid:" + notify.openid);
                //Log.Debug("微信支付结果通知:", "序列化后的total_fee（该字段返回来的单位是分）:" + notify.total_fee);
                //Log.Debug("微信支付结果通知:", "序列化后的out_trade_no:" + notify.out_trade_no);
                //Log.Debug("微信支付结果通知:", "序列化后的transaction_id:" + notify.transaction_id);
                //Log.Debug("微信支付结果通知:", "序列化后的notifyData:" + notifyData.ToJson());

                //OverAddFee(out_trade_no,openid) openid?
                //填写记录
                //using (var balancebLL = new BalanceBLL())
                //{
                //    balancebLL.PayOk(notify.out_trade_no).ConfigureAwait(true);
                //}
                

            }

                
            return View();
        }
        public ActionResult Indextest()
        {
            Log.Debug("收到通知:", "收到通知");
             

            return View();
        }

        
    }
}