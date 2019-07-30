using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
namespace WxPay2017.API.VO
{
    /// <summary>
    /// 消息 +  消息可接收人员
    /// </summary>
    public class MessagesPostData
    {
        /// <summary>
        /// 消息可接收人员
        /// </summary>
        public Dictionary<CategoryDictionary, List<string>> IdsByCategory { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public MessageData Message { get; set; }

    }
}
