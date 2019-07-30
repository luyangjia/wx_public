using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class RequestForOvertimeData
    {
        public int Id { get; set; }

        public string ApplicantId { get; set; }

        public int BuildingId { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public string ApproverId { get; set; }

        public string ApproverDesc { get; set; }

        public bool? IsOk { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CheckTIme { get; set; }
    }
}
