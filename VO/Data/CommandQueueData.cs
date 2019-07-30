using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class CommandQueueData
    {

        public int Id { get; set; }

        //public int MeterActionId { get; set; }

        public int CommandType { get; set; }

        public DateTime? CommandTime { get; set; }

        public int? SendCount { get; set; }

        public string SendSource { get; set; }

        public string CommandValue { get; set; }

        public bool? IsReply { get; set; }

        public DateTime? ReplyTime { get; set; }

        public string ReplyValue { get; set; }

        public int? CommandNum { get; set; }

        public int? CommandGroup { get; set; }

        public string GatewayIpAddress { get; set; }

        public string Rs485Addr { get; set; }

        public int? Port { get; set; }

        public bool? isSucc { get; set; }

        public bool? isErr { get; set; }

        public int Priority { get; set; }

    }

}
