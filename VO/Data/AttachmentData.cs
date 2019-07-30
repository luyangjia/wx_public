using System;
using System.Collections.Generic;

namespace WxPay2017.API.VO
{
    public class AttachmentData
    {
        public int Id { get; set; }

        public string TargetId { get; set; }

        public int AttachmentTypeId { get; set; }

        public int AttachmentFormatId { get; set; }

        public string Description { get; set; }

        public int Size { get; set; }

        public DateTime CreateTime { get; set; }


        public string Path { get; set; }


        public string OriginalName { get; set; }


        public string LogicalName { get; set; }

        public virtual DictionaryData AttachmentType { get; set; }
        public virtual DictionaryData AttachmentFormat { get; set; }
        public Object Target { get; set; }
    }
}
