using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.DAL.EmpModels
{
    [Table("Foundation.Report")]
    public partial class Report
    {
        public int Id { get; set; }

        public DateTime DataExtractTime { get; set; }

        public DateTime DataPackTime { get; set; }

        [Required]
        [StringLength(1000)]
        public string DataPackXml { get; set; }

        public DateTime DataReportTime { get; set; }

        public DateTime DataReciveTime { get; set; }

        [Required]
        [StringLength(500)]
        public string DataReciveXml { get; set; }
    }
}
