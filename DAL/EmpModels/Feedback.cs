using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WxPay2017.API.DAL.EmpModels
{
    [Table("Foundation.Feedback")]
    public partial class Feedback
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public int TypeId { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(128)]
        public string HandleUserId { get; set; }

        public DateTime? HandleTime { get; set; }

        public string HandleReply { get; set; }

        public int? Rating { get; set; }

        public string Comment { get; set; }

        public int StateId { get; set; }

        public virtual Dictionary Type { get; set; }

        public virtual Dictionary State { get; set; }

        public virtual User User { get; set; }

        public virtual User HandleUser { get; set; }
        public bool IsDeleted { get; set; }

    }
}
