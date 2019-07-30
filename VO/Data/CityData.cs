namespace WxPay2017.API.VO
{ 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
     
    public partial class CityData
    {  
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
         
        public string Name { get; set; }

        public int? ProvinceId { get; set; }

        public int? Sort { get; set; }
         
        [Display(Name = "组织机构")]
        public virtual ICollection<OrganizationData> Organizations { get; set; }

        [Display(Name = "建筑")]
        public virtual ICollection<BuildingData> Buildings { get; set; }

        public virtual ProvinceData Province { get; set; }

        public virtual ICollection<DistrictData> Districts { get; set; }
    }
}
