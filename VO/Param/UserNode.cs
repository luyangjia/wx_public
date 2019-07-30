using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WxPay2017.API.VO.Param
{
    public class UserNode
    {
        [Key] 
        public string ID { get; set; }

        //[Required]
        public string UserName { get; set; }


        public string FullName { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
    }
}