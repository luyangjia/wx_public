using EMP2016.API.VO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMP2016.API.VO.Common
{
    public interface INotifier
    {
        string Send(NotifierData msg, string userId);
        List<string> Send(NotifierData msg, List<string> userIds);
    }
}
