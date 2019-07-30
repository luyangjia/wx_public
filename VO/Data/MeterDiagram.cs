namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using WxPay2017.API.DAL.EmpModels;
    using System.Linq;
     
    public partial class MeterDiagram
    {  
        public MeterDiagram()
        {
            Normal = new List<MeterDiagram>();
            Danger = new List<MeterDiagram>();
            Warning = new List<MeterDiagram>();
           
        }
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父设备ID
        /// </summary>
        public int? ParentId { get; set; }
        public int? RelayElecState { get; set; }
        public bool? PaulElecState { get; set; }

        public string TreeId { get; set; }

        public int? Rank { get; set; }
        /// <summary>
        /// 父设备名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 设备品牌ID
        /// </summary>
        public int BrandId { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }
       
        public string BrandType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public string Access { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public int EnergyCategoryId { get; set; }

        public int HasPrivilege { get; set; }


        public string MacAddress { get; set; }
        public int HasChildren { get; set; }
        public virtual IEnumerable<MeterDiagram> Normal { get; set; }
        public virtual IEnumerable<MeterDiagram> Warning { get; set; }
        public virtual IEnumerable<MeterDiagram> Danger { get; set; }
        /// <summary>
        /// 当前表的警告级别
        /// </summary>
        public bool Enable { get; set; }

        public bool IsTurnOn { get; set; }

        public virtual ICollection<ExtensionFieldData> ExtensionFields { get; set; }
    
    }
}
