using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class UserExtensionData
    {

        public int Id { get; set; }

        public string UserId { get; set; }

        public int ColumnTypeId { get; set; }

        public string ValueStr { get; set; }

        public decimal? Value { get; set; }

    }



}
