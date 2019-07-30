
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.RequestForOvertime")]
    public partial class RequestForOvertime
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string ApplicantId { get; set; }

        public int BuildingId { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(512)]
        public string Reason { get; set; }

        [StringLength(128)]
        public string ApproverId { get; set; }

        [StringLength(512)]
        public string ApproverDesc { get; set; }

        public bool? IsOk { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CheckTIme { get; set; }

        public virtual Building Building { get; set; }

        public virtual User Applicantor { get; set; }

        public virtual User Approver { get; set; }
    }
}
