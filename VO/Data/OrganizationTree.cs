using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class OrganizationTree
    {
        public string TreeId { get; set; }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string AliasName { get; set; }

        public string Initial { get; set; }

        public int ManagerCount { get; set; }

        public int CustomerCount { get; set; }

        public bool Enable { get; set; }

        public string Description { get; set; }

        public int Rank { get; set; }
    }
}
