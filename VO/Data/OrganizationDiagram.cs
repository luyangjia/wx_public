namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using WxPay2017.API.DAL.EmpModels;
    using System.Linq;
    public partial class OrganizationDiagram
    {
        public OrganizationDiagram()
        {      
            Children = new List<OrganizationDiagram>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public string Name { get; set; }

      
        public string AliasName { get; set; }

      
        public string Initial { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public bool Enable { get; set; }

        public int HasChildren { get; set; }
       
        public string Description { get; set; }

        public virtual OrganizationDiagram Parent { get; set; }

        public virtual IEnumerable<OrganizationDiagram> Children { get; set; }

        public virtual ICollection<BuildingData> Buildings { get; set; }

        public virtual ICollection<ExtensionFieldData> ExtensionFields { get; set; }
       
    }
}
