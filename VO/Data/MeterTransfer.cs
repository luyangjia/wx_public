namespace WxPay2017.API.VO
{
    using WxPay2017.API.DAL.EmpModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    public partial class MeterTransfer
    {
        public Meter Meter { get; set; }

        public int? TargetId { get; set; }

        public string TargetName { get; set; }
    }
}
