using System;
using System.Collections.Generic;

namespace WxPay2017.API.VO
{
   public class MessageData
    {

        public int Id { get; set; }

        public int MessageTypeId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int MessageSourceTypeId { get; set; }

        public string SrcId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Url { get; set; }

        public DateTime ActiveDate { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime NotActiveDate { get; set; }
        public int? AlertLevelId { get; set; }

        public String MessageTypeName { get; set; }
        public String MessageSourceTypeName { get; set; }
        public String SenderName { get; set; }

        public Object MessageSource { get; set; }

        public virtual ICollection<MessageRecordData> MessageRecords { get; set; }
    }

    /// <summary>
    /// 目标消息数量
    /// </summary>
   public class TargetMessageNum
   {
       public int MessageNumReaded { get; set; }//已读消息
       public int MessageNumNotReaded { get; set; }//未读消息
   }
}
