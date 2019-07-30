
namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;

    public partial class UserData
    {  
        public string Id { get; set; }

        public int? OrganizationId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }
         
        public string UserName { get; set; }
         
        public string FullName { get; set; }

        public IList<UserAccountData> Account { get; set; }


        public bool IsResignOrGraduate { get; set; }

        public string ForeignId { get; set; }
        public bool? IsRightInfo { get; set; }

        public string IdentityNo { get; set; }
         
        public DateTime EnrollDate { get; set; }
        public bool? IsChecked { get; set; }
        public string StaffNo { get; set; }

        public string QQ { get; set; }

        public string WeChat { get; set; }

        public int? UserType { get; set; }

        public bool? Gender { get; set; }

        public string UserTypeName { get; set; }

        public virtual ICollection<OrganizationData> Organizations { get; set; }
        public virtual OrganizationData Org { get; set; }
        public virtual ICollection<BuildingData> Buildings { get; set; }

        public virtual ICollection<RoleData> Roles { get; set; }
       

    }
    public class UserLittleData
    {
        public string Id { get; set; }

        public int? OrganizationId { get; set; }
        public bool? IsChecked { get; set; }
        public string Name { get; set; }

    }
    public class UserShortData
    {
        public string Id { get; set; }

        public int? OrganizationId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public IList<UserAccountData> Account { get; set; }


        public bool IsResignOrGraduate { get; set; }
        public bool? IsRightInfo { get; set; }
        public string ForeignId { get; set; }

        public string IdentityNo { get; set; }

        public DateTime EnrollDate { get; set; }
    }


    public class WxUserData
    {

        public int buildingId;
        public string building;
        public string Level;
        public string Room;
        public decimal Balance;
        public string userid;
        public string StaffNo;
        public string UserName;
        public string FullName;
        public string PhoneNumber;

        public string openid;
        public string Result;

        /// <summary>
        /// 缴费记录
        /// </summary>
        public ICollection<WxUserFee> Feelist { get; set; }


    }
    /// <summary>
    /// 缴费记录
    /// </summary>
    public class WxUserFee
    {
        public DateTime? Feedata;
        public decimal FeeAmount;
        public bool? IsSynchro;
    }




}
