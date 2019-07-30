using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.DAL.AuthModels
{
    [Table("User.User")]
    public class ApplicationUser : IdentityUser
    {
        public int? OrganizationId { get; set; }
        [StringLength(128)]
        public string FullName { get; set; }

        //public string PasswordHash { get; set; }
        //public string UserName { get; set; }


        public bool IsResignOrGraduate { get; set; }

        public string ForeignId { get; set; }
        public bool? IsRightInfo { get; set; }
        public string IdentityNo { get; set; }

        public DateTime EnrollDate { get; set; }

        public string StaffNo { get; set; }
    }

}
