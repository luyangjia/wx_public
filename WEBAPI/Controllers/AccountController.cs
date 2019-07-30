using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using WxPay2017.API.BLL;
using WxPay2017.API.VO;
using WxPay2017.API.WEBAPI.Models;

namespace WxPay2017.API.WEBAPI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        
        public ActionResult Index()
        {
            GetOpenid getopenid = ReGetOpenId();
            WxUserData userdata=null;
            if (getopenid.status == "OK")
            {
               
                //拿到了openid,去数据库看是否有绑定，有绑定就获得用户信息。 
                 using (var userBll = new UserBLL())
                { 
                      userdata = userBll.GetUserBlanceInfo(getopenid.openid); 
                }

                if (userdata.Result == "该用户未绑定")
                { 
                   // System.Web.HttpContext.Current.Response.Redirect("Account/Register");
                    return View("Register");
                }


            }
            return View(userdata);



        }
        /// <summary>
        /// 获得用户登录进来的openid
        /// </summary>
        public static GetOpenid ReGetOpenId()
        {
            string CodeUrl = "";
            string url = "";
            GetOpenid getopenid = new GetOpenid();

            //先要判断是否是获取code后跳转过来的
            if (System.Web.HttpContext.Current.Request.QueryString["code"] == "" || System.Web.HttpContext.Current.Request.QueryString["code"] == null)
            {
                //Code为空时，那就先去先获取Code   
                url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;//获取当前url
                url = System.Web.HttpUtility.UrlEncode(url); //对url进行编码
                CodeUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WxPayConfig.APPID + "&redirect_uri=" + url + "?action=viewtest&response_type=code&scope=snsapi_base&state=1#wechat_redirect");

                System.Web.HttpContext.Current.Response.Redirect(CodeUrl);//先跳转到微信的服务器，取得code后会跳回来这页面的

            }
            else
            {
                // 跳回来后根据Code获取用户的openid、access_token 

                url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WxPayConfig.APPID + "&secret=" + WxPayConfig.APPSECRET + "&code=" + System.Web.HttpContext.Current.Request.QueryString["Code"] + "&grant_type=authorization_code";

                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //创建HttpClient 
                using (var http = new HttpClient(handler))
                {
                    HttpResponseMessage response = http.GetAsync(url).Result;  
                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = response.Content.ReadAsStringAsync().Result;
                        //序列化  
                        OAuth_Token ac = new OAuth_Token();
                        ac = JSONHelper.JsonDeserialize<OAuth_Token>(responseJson);

                        WxPayData data = new WxPayData();
                        data.SetValue("openid", ac.openid);
                        data.SetValue("access_token", ac.access_token);
                        data.SetValue("expires_in", ac.expires_in);
                        data.SetValue("scope", ac.scope);
                        data.SetValue("refresh_token", ac.refresh_token);

                      System.Web.HttpContext.Current.Session["openid"] = ac.openid;
                        getopenid.openid = ac.openid;
                        getopenid.code = System.Web.HttpContext.Current.Request.QueryString["code"];
                        getopenid.status = "OK"; 
                    }
                    else
                    {

                        getopenid.code = System.Web.HttpContext.Current.Request.QueryString["code"];
                        getopenid.status = "error,StatusCode:" + response.StatusCode.ToString();
                    }

                }
                 



            }


            return getopenid;
        }




        public ActionResult Register()
        { 
         
            return View();
        } 

      
        [ValidateAntiForgeryToken]
      
        public ActionResult SaveData(RegistUserViewModel registdata)
        {
            string phone = registdata.phoneno;
            string studentno = registdata.studentno;
           
            string result = "";
            if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(studentno))
            {
                using (var userBll = new UserBLL())
                {
                   // Log.Debug("result:", "phone:" + phone + ",studentno:" + studentno + ",openid:" + (string)System.Web.HttpContext.Current.Session["openid"]);
                    result = userBll.UpdateOpenid(phone.Replace("'", "''"), studentno.Replace("'", "''"), (string)System.Web.HttpContext.Current.Session["openid"]);
                }

            }

           // Log.Debug("result:", result);
            if (result=="OK")
            {
                System.Web.HttpContext.Current.Response.Redirect("Index");
                return View(); 
            } 
            else
            {
                ViewBag.error = "绑定失败，请输入的信息不存在！";
                return View("Register"); 
            }

          //  return Content(last);
           
            
        }






    }















}