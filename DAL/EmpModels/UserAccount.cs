namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User.UserAccount")]
    public partial class UserAccount
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Balance { get; set; }

        public int BalanceTypeId { get; set; }

        public DateTime AddTime { get; set; }

        public bool Enable { get; set; }

        public virtual Dictionary BalanceType { get; set; }

        public virtual User User { get; set; }
    }
}
