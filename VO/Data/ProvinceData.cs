namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProvinceData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Sort { get; set; }

        public string Remark { get; set; }


        [Display(Name = "��֯����")]
        public virtual ICollection<OrganizationData> Organizations { get; set; }

        [Display(Name = "����")]
        public virtual ICollection<BuildingData> Buildings { get; set; }

        [Display(Name = "����")]
        public virtual ICollection<CityData> Cities { get; set; }
    }
}
