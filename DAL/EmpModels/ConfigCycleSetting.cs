
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.ConfigCycleSetting")]
    public partial class ConfigCycleSetting
    {
        public int Id { get; set; }

        public int ConfigId { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public virtual MonitoringConfig MonitoringConfig { get; set; }

        public string Description { get; set; }
    }
}
