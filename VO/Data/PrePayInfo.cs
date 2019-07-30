using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
namespace WxPay2017.API.VO
{
    public class PrePayInfo
    {
        public CategoryDictionary Category { get; set; }//对象分类,building:orgrazation
        public List<int> Ids { get; set; }//对象id集合
        public decimal Value { get; set; }//金额
        public string Desc { get; set; }//缴费说明
    }
}
