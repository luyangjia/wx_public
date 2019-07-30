using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;


namespace WxPay2017.API.WEBAPI
{

    public class GetOpenid
    { 
       
        public string openid { get; set; }
        
        public string status { get; set; }
        public string code { get; set; } 
    }


    public class OAuth_Token
    {
         
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public string expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 用户唯一标识,请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户授权作用域
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 微信生成的订单号，返回给商户
        /// </summary>
        public string transactionId { get; set; }
        /// <summary>
        /// 商户平台的订单号（自己生成）
        /// </summary>
        public string out_trade_no { get; set; }

    }


    public class JSONHelper
    { 
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

    }

    /// <summary>
    /// 微信回调数据模型
    /// </summary>
    public class NotifyModel
    {
      /// <summary>
      /// 公众号ID
      /// </summary>
          public string appid { get; set; }
          public string attach { get; set; }
          public string bank_type { get; set; }
          public string cash_fee { get; set; }
          public string fee_type { get; set; }
          public string is_subscribe { get; set; }
          public string mch_id { get; set; }
          public string nonce_str { get; set; }
        /// <summary>
        /// 客户微信ID
        /// </summary>
          public string openid { get; set; }
        /// <summary>
        /// 我们生成的订单号
        /// </summary>
          public string out_trade_no { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
          public string result_code { get; set; }
         public string return_code { get; set; }
         /// <summary>
         /// 签名
         /// </summary>
         public string sign { get; set; }
          public string time_end { get; set; }
          /// <summary>
          /// 金额（这里注意单位是分）
          /// </summary>
          public string total_fee { get; set; }
          public string trade_type { get; set; }
          /// <summary>
          /// 微信生成的订单号
          /// </summary>
         public string transaction_id { get; set; }
       
     
    }
 
 
    /// <summary>
    /// 统一下单的信息
    /// </summary>
      public class OrderModel
    {
      
          public string body { get; set; }
          public string attach { get; set; }
          public string out_trade_no { get; set; }
          public string total_fee { get; set; }
          public string time_start { get; set; }
          public string time_expire { get; set; }
          public string goods_tag { get; set; }
          public string trade_type { get; set; }
          public string openid { get; set; } 
           
       
     
    }
 
 
      

}