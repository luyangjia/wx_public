using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Data
{
    public partial class ReportData
    {

        public int Id { get; set; }

        public DateTime DataExtractTime { get; set; }

        public DateTime DataPackTime { get; set; }

        public string DataPackXml { get; set; }

        public DateTime DataReportTime { get; set; }

        public DateTime DataReciveTime { get; set; }

        public string DataReciveXml { get; set; }

    }



}
