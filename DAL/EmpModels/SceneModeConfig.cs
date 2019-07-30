
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.SceneModeConfig")]
    public partial class SceneModeConfig
    {
        public int Id { get; set; }

        public int SceneModeId { get; set; }

        public int ConfigId { get; set; }

        public virtual MonitoringConfig MonitoringConfig { get; set; }

        public virtual SceneMode SceneMode { get; set; }
    }
}
