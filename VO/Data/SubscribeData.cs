using System;
using System.Collections.Generic;

namespace WxPay2017.API.VO
{
  public  class SubscribeData
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TargetTypeId { get; set; }

        public string TargetId { get; set; }

        public bool Enabled { get; set; }

        public int MessageTypeId { get; set; }
        public int ReceivingModelId { get; set; }
        public virtual DictionaryData Type { get; set; }
        public virtual DictionaryData TargetType { get; set; }
        public virtual DictionaryData MessageType { get; set; }
        public virtual DictionaryData ReceivingModel { get; set; }
        public object Target { get; set; }
    }
}
