namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.City")]
    public partial class City
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {
            Organizations = new HashSet<Organization>();
            Buildings = new HashSet<Building>();
            Districts = new HashSet<District>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? ProvinceId { get; set; }

        public int? Sort { get; set; }
         
        [Display(Name = "组织机构")]
        public virtual ICollection<Organization> Organizations { get; set; }

        [Display(Name = "建筑")]
        public virtual ICollection<Building> Buildings { get; set; }

        public virtual Province Province { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
