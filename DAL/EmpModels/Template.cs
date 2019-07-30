//namespace WxPay2017.API.DAL.EmpModels
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Data.Entity.Spatial;

//    [Table("Message.Template")]
//    public partial class Template
//    {
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
//        public Template()
//        {
//        }

//        [Key]
//        public long Id { get; set; }

//        [StringLength(256)]
//        [Required]
//        public string Name { get; set; }

//        public int Category { get; set; }

//        public Guid? ApplicationId { get; set; }
         
//        [StringLength(50)]
//        [Required]
//        public string Subject { get; set; }
         
//        [Column(TypeName = "text")]
//        [Required]
//        public string Body { get; set; }

//        [Column(TypeName = "text")]
//        [Required]
//        public string Parameters { get; set; }

//        [Column(TypeName = "text")]
//        [Required]
//        public string Description { get; set; }
         
//        public virtual Dictionary CategoryDict { get; set; }

//    }
//}
