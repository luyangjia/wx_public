using System.ComponentModel.DataAnnotations;
namespace WxPay2017.API.VO
{
    public partial class RoleData
    {  
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }         
    }
}
