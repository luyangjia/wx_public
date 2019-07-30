
namespace WxPay2017.API.VO
{
    using System.Collections.Generic;
    public partial class PermissionData
    { 

        public int Id { get; set; }


        public int? ParentId { get; set; }


        public string Title { get; set; }


        public string Area { get; set; }


        public string Controller { get; set; }


        public string Action { get; set; }


        public string Value { get; set; }

        public string HttpMethod { get; set; }

        public string Url { get; set; }


        public bool IsNav { get; set; }


        public bool Actived { get; set; }

        public bool Disabled { get; set; }

        public int Sort { get; set; }


        public string Description { get; set; }


        public int CategoryID { get; set; }


        public string CategoryTitle { get; set; }


        public string Icon { get; set; }

        public virtual ICollection<PermissionData> Children { get; set; }

        public virtual PermissionData Parent { get; set; }

        public virtual ICollection<ExtensionFieldData> ExtensionFields { get; set; }
         
    }
}
