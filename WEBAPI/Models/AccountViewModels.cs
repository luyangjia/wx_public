using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WxPay2017.API.WEBAPI.Models
{
  

    
     

    public class AmountViewModel
    {
        

        [Required]
        [StringLength(10, ErrorMessage = "{0} 必须至少包含 {2} 个。", MinimumLength = 1)]
        [DataType(DataType.Currency)]
        [Display(Name = "金额")]
        public double Amount { get; set; }

        
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }


    public class RegistUserViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 5)]
        [Display(Name = "学号")]
        public string studentno { get; set; }


        [Required]
        [StringLength(11, ErrorMessage = "手机号是11位", MinimumLength = 11)]
        [Display(Name = "手机号")]
        public string phoneno { get; set; }
      
    }

}
