using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Data
{
    public partial class StatisticsSubscribeData
    {

        public int Id { get; set; }

        public int CategoryDictionary { get; set; }

        public int TargetId { get; set; }

        public int EnergyCategoryId { get; set; }

        public string Emails { get; set; }

        public DateTime? SubDate { get; set; }

        //public bool? IsPeriodMonth { get; set; }
        public string Period { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

    }



}
