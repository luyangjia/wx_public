using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class ExtensionFieldData
    {
        public int Id { get; set; }
        public string Database { get; set; }
         
        public string Schema { get; set; }
         
        public string Table { get; set; }
         
        public string Column { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public int JoinId { get; set; }
         
        public string Value { get; set; }
         
        public string ValueType { get; set; }
         
        public string ChineseName { get; set; }
         
        public string EnglishName { get; set; }
         
        public bool Enable { get; set; }
         
        public string Description { get; set; }

        public virtual Object RelatedObject { get; set; }
    }
}
