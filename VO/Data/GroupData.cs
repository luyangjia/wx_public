using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class GroupData
    {
        public int Id { get; set; }

        public int GroupTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enable { get; set; }

        public string RegeneratorId { get; set; }

        public string RegeneratorName { get; set; }

        public DateTime? UpdatingTime { get; set; }

        public string GroupTypeName { get; set; }

        public UserData Regenerator { get; set; }


    }
}
