using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class SocketData
    {
        public int? BuildingId { get; set; }//建筑Id    
        public int? GatewayId { get; set; }//网关Id
        public string Type { get; set; }//开关类型  Dictionary.Action.EquText
        public int? Port { get; set; }//端口
        public int? MeterId { get; set; }//设备Id
        public string IpAddress { get; set; }//IP地址
        public string PhyAddress { get; set; }//物理MAC地址
        public string GbCode { get; set; }//国标编码
        public int? Value { get; set; }  
    }
}
