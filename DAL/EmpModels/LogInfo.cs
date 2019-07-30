
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.LogInfo")]
    public partial class LogInfo
    {
        public int Id { get; set; }

        public int? MeterId { get; set; }

        public int? GroupId { get; set; }

        public int? ActionId { get; set; }

        public DateTime? UpdatingTime { get; set; }

        public bool? IsSuccess { get; set; }

        [StringLength(128)]
        public string OperatorId { get; set; }

        [StringLength(256)]
        public string OperatorName { get; set; }

        [StringLength(256)]
        public string ConfigName { get; set; }

        public virtual Dictionary Dictionary { get; set; }

        public virtual Group Group { get; set; }

        public virtual Meter Meter { get; set; }

        public virtual User User { get; set; }
    }
}
