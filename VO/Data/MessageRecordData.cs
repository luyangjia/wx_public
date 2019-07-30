using System;
using System.Collections.Generic;

namespace WxPay2017.API.VO
{
    public class MessageRecordData
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string UserId { get; set; }

        public bool IsReaded { get; set; }

        public bool IsEnable { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? ReadedTime { get; set; }

        public virtual MessageData Message { get; set; }

        public virtual UserData User { get; set; }
    }

    public class ReturnMessageRecordData
    {
        public List<MessageRecordDetailData> MessageRecordDetailData { get; set; }
        public bool IsNextPage { get; set; } //判断是否有下一页 true为有
        public int MessageNum { get; set; } //消息总数
    }

    public class MessageRecordDetailData
    {
        public MessageRecordData messageRecordData { get; set; }
        public bool isdone { get; set; }    //判断消息是否已经处理完成  true 为已处理
    }
}
