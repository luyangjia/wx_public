using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Common
{ 
    /// <summary>
    /// 消息模式
    /// </summary>
    [Flags]
    public enum NoticeMode
    {   //消息模式
        /// <summary>
        /// 网站消息
        /// </summary>
        [Display(Name = "网站消息")]
        Web = 0x001, //WEB

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Display(Name = "电子邮箱")]
        Email = 0x002,  //Email

        /// <summary>
        /// 短信
        /// </summary>
        [Display(Name = "短信")]
        SMS = 0x004,    //手機短信

        /// <summary>
        /// 微信
        /// </summary>
        [Display(Name = "微信")]
        WeChat = 0x008,      //微信

        /// <summary>
        /// 电话联系
        /// </summary>
        [Display(Name = "电话联系")]
        Call = 0x010,       //电话联系
    }
     
}
