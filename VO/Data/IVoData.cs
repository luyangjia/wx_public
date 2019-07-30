using WxPay2017.API.VO.Param;
using System.Collections.Generic;
namespace WxPay2017.API.VO
{
    public partial class IVoData<T>
    {
        public ICollection<T> List { get; set; }
        public Pagination Pagination { get; set; }
    }
}
