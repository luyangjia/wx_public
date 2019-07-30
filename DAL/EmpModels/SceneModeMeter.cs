
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.SceneModeMeter")]
    public partial class SceneModeMeter
    {
        public int Id { get; set; }

        public int SceneModeId { get; set; }

        public int? GroupId { get; set; }

        public int? BuildingId { get; set; }

        public int? MeterId { get; set; }

        public DateTime SettingTime { get; set; }

        [StringLength(128)]
        public string OperatorId { get; set; }

        [StringLength(256)]
        public string OperatorName { get; set; }

        public virtual Building Building { get; set; }

        public virtual SceneMode SceneMode { get; set; }

        public virtual Group Group { get; set; }

        public virtual Meter Meter { get; set; }

        public virtual User User { get; set; }
    }
}
