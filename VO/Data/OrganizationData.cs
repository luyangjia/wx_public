using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;

namespace WxPay2017.API.VO
{
    public class OrganizationData
    {
        public OrganizationData()
        {
            //ExtensionFields = new HashSet<ExtensionFieldData>();
        }
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public string Name { get; set; }

        public string AliasName { get; set; }

        public string Initial { get; set; }

        public int Type { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public int? DistrictId { get; set; }

        public bool Enable { get; set; }

        public string Description { get; set; }

        public OrganizationData Parent { get; set; }

        public int HasChildren { get; set; }

        public virtual DictionaryData TypeDict { get; set; }

        public virtual ICollection<OrganizationData> Children { get; set; }
        public virtual ICollection<OrganizationData> Descendants { get; set; }
        public virtual ICollection<OrganizationData> Ancestors { get; set; }

        public virtual ICollection<UserData> Users { get; set; }

        public virtual ICollection<ExtensionFieldData> ExtensionFields { get; set; }

        public virtual ICollection<BuildingData> Buildings { get; set; }

        public virtual ProvinceData Province { get; set; }
        public virtual CityData City { get; set; }
        public virtual DistrictData District { get; set; }
    }

    public class OrganizationShortData
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public string Name { get; set; }

        public string AliasName { get; set; }

        public string Initial { get; set; }

        public int Type { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public int? DistrictId { get; set; }

        public bool Enable { get; set; }

        public string Description { get; set; }
    }
}
