using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MonthBalance
    {
         public decimal Total{get;set;}
         public decimal WaterConsumption {get;set;}

         public decimal? PrePay = null;
         public decimal  WaterAmount{get;set;}
         public decimal PowerConsumption{get;set;}
         public decimal PowerAmount{get;set;}
         public decimal CashCharge{get;set;}
         public decimal Recharge {get;set;}
         public decimal Subsidy {get;set;}
         public DateTime Time { get; set; }
         public String TimeStr = null;
         public List<int> Ids = new List<int>();
    }
}
