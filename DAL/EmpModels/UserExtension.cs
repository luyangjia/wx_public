using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.DAL.EmpModels
{
    [Table("User.UserExtension")]
    public partial class UserExtension
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public int ColumnTypeId { get; set; }

        [StringLength(128)]
        public string ValueStr { get; set; }

        public decimal? Value { get; set; }

        public virtual Dictionary ColumnType { get; set; }

        public virtual User User { get; set; }
    }
}
