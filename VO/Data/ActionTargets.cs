using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
namespace WxPay2017.API.VO
{
    public class ActionTargets
    {
        /// <summary>
        /// building,meter
        /// </summary>
        public CategoryDictionary Category { get; set; }
       /// <summary>
       /// 0网关，1电表，2水表,category为meter时不用
       /// </summary>
        public int MeterType{ get; set; }
        public List<int> Ids { get; set; }
        /// <summary>
        /// category为meter时不用，如果是房间，则房间中是否必须有能耗用户
        /// </summary>
        public bool isMustHaveStudents = false;

    }
}
