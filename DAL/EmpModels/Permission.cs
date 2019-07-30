namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User.Permission")]
    public partial class Permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Permission()
        {
            Roles = new HashSet<Role>();
            Children = new HashSet<Permission>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Area { get; set; }

        [Required]
        [StringLength(50)]
        public string Controller { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        [Required]
        [StringLength(1024)]
        public string Value { get; set; }

        public string HttpMethod { get; set; }

        [Required]
        [StringLength(1024)]
        public string Url { get; set; }

        public bool IsApi { get; set; }

        public bool IsNav { get; set; }

        public bool Actived { get; set; }

        public bool Disabled { get; set; }

        public int Sort { get; set; }

        public int Version { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryTitle { get; set; }

        [Required]
        [StringLength(50)]
        public string Icon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Permission> Children { get; set; }
        public virtual Permission Parent { get; set; }
    }
}
