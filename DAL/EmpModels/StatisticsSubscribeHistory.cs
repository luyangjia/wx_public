namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message.StatisticsSubscribeHistory")]
    public partial class StatisticsSubscribeHistory
    {
        public int Id { get; set; }

        public int StatisticsSubscribeId { get; set; }

        public DateTime SendDate { get; set; }

        public virtual StatisticsSubscribe StatisticsSubscribe { get; set; }
    }
}
