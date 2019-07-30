using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class  CollectionAccountInfo
    {
        public int OrgId{get;set;}
        /// <summary>
        /// 收款者的合作者身份Id
        /// </summary>
        public string ParterId { get; set; }
        /// <summary>
        /// 支付宝账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 私钥,保存在RSA目录下，格式如rsa_private_key.pem
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// 支付宝公钥保存在RSA目录下，格式如rsa_public_key.pem
        /// </summary>
        public string PubliceKey { get; set; }
    }
}
