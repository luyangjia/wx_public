using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
    public class ForgotUserNode
    {
        public string UserId { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
         
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public string Email { get; set; }


        public string PhoneNumber { get; set; }

        public string IdentityNo { get; set; }

        public string Token { get; set; }

    }
}
