namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Building.Building")]
    public partial class Building
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Building()
        {
            Children = new HashSet<Building>();
            Meters = new HashSet<Meter>();
            Users = new HashSet<User>();
            MeterGroups = new HashSet<MeterGroup>();
            SceneModeMeters = new HashSet<SceneModeMeter>();
            RequestForOvertimes = new HashSet<RequestForOvertime>();
            BuildingMeterTypeUsers = new HashSet<BuildingMeterTypeUser>();
            Maintenances = new HashSet<Maintenance>();
            ConfigDetails = new HashSet<ConfigDetail>();
        }
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [StringLength(128)]
        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public int? OrganizationId { get; set; }

        public int BuildingCategoryId { get; set; }

        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }

        [StringLength(16)]
        public string GbCode { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string AliasName { get; set; }

        [StringLength(16)]
        public string Initial { get; set; }

        public int Type { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public int? DistrictId { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public decimal? TotalArea { get; set; }

        public decimal? WorkingArea { get; set; }

        public decimal? LivingArea { get; set; }

        public decimal? ReceptionArea { get; set; }

        public bool Enable { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public int? Year { get; set; }

        public int? UpFloor { get; set; }

        public int Sort { get; set; }

        public int? MaxCustomerCount { get; set; }

        public int? Purpose { get; set; }

        public virtual Dictionary DictionaryEnergyUserType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Building> Children { get; set; }

        public virtual Building Parent { get; set; }

        public virtual Coordinate Coordinate2d { get; set; }

        public virtual Coordinate Coordinate3d { get; set; }

        public virtual Coordinate CoordinateMap { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual Dictionary BuildingCategoryDict { get; set; }

        public virtual Dictionary TypeDict { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Meter> Meters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        public virtual Province Province { get; set; }

        public virtual City City { get; set; }

        public virtual District District { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeterGroup> MeterGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SceneModeMeter> SceneModeMeters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestForOvertime> RequestForOvertimes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuildingMeterTypeUser> BuildingMeterTypeUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maintenance> Maintenances { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigDetail> ConfigDetails { get; set; }
    }
}
