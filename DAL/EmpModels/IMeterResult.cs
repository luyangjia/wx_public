using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.DAL.EmpModels
{
    public interface IMeterResult
    {
        Guid Id { get; set; }

        int MeterId { get; set; }

        DateTime StartTime { get; set; }

        DateTime FinishTime { get; set; }

        decimal Total { get; set; }

        string Unit { get; set; }

        int ParameterId { get; set; }

        int Status { get; set; }

        Dictionary StatusDict { get; set; }

        Meter Meter { get; set; }

        Parameter Parameter { get; set; }
    }
}
