using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Configuration;
using System.Collections.Generic;
using EMP2016.API.DAL.EmpModels;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Collections;

namespace EMP2016.API.VO.Common
{

    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.4
    /// 修改日期：2016-03-08
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// </summary>
    public  class ConfigAliPay
    {
        /// <summary>
        /// 根据当前缴费用户，获取最近的上级收款账号信息
        /// 收款账号规则：根据建筑获取其最近有设置上级收款账号的组织机构
        /// 本建筑如果无归属机构，则找上级建筑，直到有机构，再进行上溯找到设置上级收款账号的组织机构
        /// 收款账号规则：SecurityStamp字段依次保存：
        /// partner，seller，private_key，alipay_public_key
        /// 秘钥文件单独上传，格式：RSA\\rsa_private_key.pem
        /// 用，隔开
        /// <param name="user"></param>
        public bool SetConfig(Building building)
        {
            Organization org = null;
            for (; ; )
            {
                if (building.OrganizationId != null)
                {
                    org = building.Organization;
                    break;
                }
                else
                {
                    if (building.ParentId != null)
                        building = building.Parent;
                    else
                        return false;
                }
            }
            return SetConfig(org);
        }
        public bool SetConfig(Organization org)
        {
            for (; ; )
            {
                var users = org.Users.Where(o => o.Roles.Any(c => c.Name == "缴费账户")).ToList();
                if (users.Count() >0 && users[0].SecurityStamp != null && users[0].SecurityStamp.Contains(",") && users[0].SecurityStamp.EndsWith(".pem"))
                {
                    //找到缴费账户
                    string[] strs = users[0].SecurityStamp.Split(',');
                    partner = strs[0];
                    seller_id = partner;
                    SELLER = strs[1];
                    private_key = HttpRuntime.AppDomainAppPath.ToString() + strs[2];
                    alipay_public_key = HttpRuntime.AppDomainAppPath.ToString() + strs[3];
                    return true;
                }
                else
                {
                    if (org.ParentId != null)
                        org = org.Parent;
                    else
                        return false;
                }
            }
        }
        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        //public static string partner = ConfigurationManager.AppSettings["PARTNER"];
        public  string partner;

        // 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
        public  string seller_id ;
        // 商户收款账号
        //String SELLER = ConfigurationManager.AppSettings["SELLER"];
        public String SELLER;
        //商户的私钥文件路径,原始格式，RSA公私钥生成：https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.nBDxfy&treeId=58&articleId=103242&docType=1
        //public static string private_key = HttpRuntime.AppDomainAppPath.ToString() + "RSA\\rsa_private_key.pem";
        public  string private_key;
        //支付宝的公钥文件路径，查看地址：https://b.alipay.com/order/pidAndKey.htm 
        //public static string alipay_public_key = HttpRuntime.AppDomainAppPath.ToString() + "RSA\\rsa_public_key.pem";
        public  string alipay_public_key;
        // 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        public  string notify_url = ConfigurationManager.AppSettings["BackUrl"];

        // 页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
        public  string return_url = ConfigurationManager.AppSettings["BackUrl"];

        // 签名方式
        public  string sign_type = "RSA";

        // 调试用，创建TXT日志文件夹路径，见AlipayCore.cs类中的LogResult(string sWord)打印方法。
        public  string log_path = HttpRuntime.AppDomainAppPath.ToString() + "";

        // 字符编码格式 目前支持 gbk 或 utf-8
        public  string input_charset = "utf-8";

        // 支付类型 ，无需修改
        public  string payment_type = "1";

        // 调用的接口名，无需修改
        public  string service = "create_direct_pay_by_user";

        //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        //↓↓↓↓↓↓↓↓↓↓请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        //防钓鱼时间戳  若要使用请调用类文件submit中的Query_timestamp函数
        public  string anti_phishing_key = "";

        //客户端的IP地址 非局域网的外网IP地址，如：221.0.0.1
        public  string exter_invoke_ip = "";

        //↑↑↑↑↑↑↑↑↑↑请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

    }
}