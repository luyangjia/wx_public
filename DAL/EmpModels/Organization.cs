namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Organization.Organization")]
    public partial class Organization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organization()
        {
            Buildings = new HashSet<Building>();
            Children = new HashSet<Organization>();
            Users = new HashSet<User>();
            //Fields = new HashSet<ExtensionField>();
            ConfigDetails = new HashSet<ConfigDetail>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [StringLength(128)]
        public string TreeId { get; set; }

        public int? Rank { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string AliasName { get; set; }

        [StringLength(16)]
        public string Initial { get; set; }

        public int Type { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public int? DistrictId { get; set; }

        public bool Enable { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Building> Buildings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Organization> Children { get; set; }

        public virtual Organization Parent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        public virtual Dictionary TypeDict { get; set; }

        public virtual Province Province { get; set; }

        public virtual City City { get; set; }

        public virtual District District { get; set; }

        //public virtual ICollection<ExtensionField> Fields { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigDetail> ConfigDetails { get; set; }
    }
}
