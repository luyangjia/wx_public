using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 返回未读消息数据类
    /// </summary>
    /// <param name="messagesId">返回消息类型父类Id</param>
    /// <param name="messagesNum">返回消息数量</param>
    /// <param name="messagesContent">返回消息内容</param>
    public class MessagesNotReadData
    {
        public int MessagesId { get; set; }
        public int MessagesNum { get; set; }
        public List<MessageRecordData> MessagesContent { get; set; }
    }
}
