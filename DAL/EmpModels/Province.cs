namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.Province")]
    public partial class Province
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Province()
        {
            Organizations = new HashSet<Organization>();
            Buildings = new HashSet<Building>();
            Cities = new HashSet<City>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? Sort { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }


        [Display(Name = "组织机构")]
        public virtual ICollection<Organization> Organizations { get; set; }

        [Display(Name = "建筑")]
        public virtual ICollection<Building> Buildings { get; set; }

        [Display(Name = "城市")]
        public virtual ICollection<City> Cities { get; set; }
    }
}
