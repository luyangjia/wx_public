
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Meter.CommandQueue")]
    public partial class CommandQueue
    {
        public int Id { get; set; }

        //public int MeterActionId { get; set; }    //修改为CommandGroup

        public int CommandType { get; set; }

        public DateTime? CommandTime { get; set; }

        public int? SendCount { get; set; }

        public string SendSource { get; set; }

        public string CommandValue { get; set; }

        public bool? IsReply { get; set; }

        public DateTime? ReplyTime { get; set; }

        [StringLength(1000)]
        public string ReplyValue { get; set; }

        public int? CommandNum { get; set; }

        public int? CommandGroup { get; set; }

        [StringLength(50)]
        public string GatewayIpAddress { get; set; }

        [StringLength(50)]
        public string Rs485Addr { get; set; }

        public int? Port { get; set; }

        public bool? isSucc { get; set; }

        public bool? isErr { get; set; }


        public int Priority { get; set; }

        public virtual MetersAction MetersAction { get; set; }
    }
}
