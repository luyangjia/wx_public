using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
    public class SearchNode<T>
    {
        //public Pagination pagination { get; set; }
         
        public ICollection<T> Ids { get; set; }
       
    }
}
