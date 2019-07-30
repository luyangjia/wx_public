namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.Coordinate")]
    public partial class Coordinate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coordinate()
        {
            BuildingsBy2d = new HashSet<Building>();
            BuildingsBy3d = new HashSet<Building>();
            BuildingsByMap = new HashSet<Building>();
            MetersBy2d = new HashSet<Meter>();
            MetersBy3d = new HashSet<Meter>();
            MetersByMap = new HashSet<Meter>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public int Type { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public string Points { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "����2D����")]
        public virtual ICollection<Building> BuildingsBy2d { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "����3D����")]
        public virtual ICollection<Building> BuildingsBy3d { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "������ͼ����")]
        public virtual ICollection<Building> BuildingsByMap { get; set; }

        public virtual Dictionary TypeDict { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "�豸2D����")]
        public virtual ICollection<Meter> MetersBy2d { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "�豸3D����")]
        public virtual ICollection<Meter> MetersBy3d { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name="�豸��ͼ����")]
        public virtual ICollection<Meter> MetersByMap { get; set; }
    }
}
