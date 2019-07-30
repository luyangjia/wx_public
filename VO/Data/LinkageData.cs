using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 联动操作数据
    /// </summary>
    public class LinkageData
    {
        public CategoryDictionary Category{get;set;}//对象分类 
        public List<int> Ids {get;set;}//对象id
        public int ActionId {get;set;}//动作id
        public int? Value { get; set; }//设备值
        public bool IsAction {get;set;}//是否操作
    }
}
