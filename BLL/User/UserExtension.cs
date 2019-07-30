using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class UserExtension
    {

        public static UserData ToViewData(this User node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var results = new UserData()
            {
                Id = node.Id,
                OrganizationId = node.OrganizationId,
                Email = node.Email,
                EmailConfirmed = node.EmailConfirmed,
                //PasswordHash = node.PasswordHash,
                //SecurityStamp = node.SecurityStamp,
                PhoneNumber = node.PhoneNumber,
                PhoneNumberConfirmed = node.PhoneNumberConfirmed,
                TwoFactorEnabled = node.TwoFactorEnabled,
                LockoutEndDateUtc = node.LockoutEndDateUtc,
                LockoutEnabled = node.LockoutEnabled,
                AccessFailedCount = node.AccessFailedCount,
                UserName = node.UserName,
                FullName = node.FullName,
                IdentityNo = node.IdentityNo,
                IsResignOrGraduate = node.IsResignOrGraduate,
                IsRightInfo=node.IsRightInfo,
                ForeignId = node.ForeignId,
                EnrollDate = node.EnrollDate,
                StaffNo = node.StaffNo,
                QQ = node.QQ,
                WeChat = node.WeChat,
                UserType = node.UserType,
                Gender = node.Gender ?? true,
                Organizations = ((suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization) ? node.Organizations.ToList().Select(x => x.ToViewData()).ToList() : null,
                Buildings = ((suffix & CategoryDictionary.Building) == CategoryDictionary.Building) ? node.Buildings.ToList().Select(x => x.ToViewData()).ToList() : null,
                Roles = ((suffix & CategoryDictionary.Role) == CategoryDictionary.Role) ? node.Roles.ToList().Select(x => x.ToViewData()).ToList() : null
            };
            if ((suffix & CategoryDictionary.Org) == CategoryDictionary.Org)
            {
                OrganizationBLL OrganizationBLL = new BLL.OrganizationBLL();
                results.Org = (results.OrganizationId != null && (suffix & CategoryDictionary.Org) == CategoryDictionary.Org) ? OrganizationBLL.Find(results.OrganizationId).ToViewData() : null;
            }

            if ((suffix & CategoryDictionary.Account) == CategoryDictionary.Account)
            {
                UserAccountBLL userAccountBLL = new UserAccountBLL();
                results.Account = userAccountBLL.Filter(o => o.UserId == node.Id && o.Enable == true).ToViewList();
            }
            
            if ((suffix & CategoryDictionary.UserType) == CategoryDictionary.UserType)
            {
                if(results.UserType!=null)
                {
                    DictionaryBLL dictBLL = new DictionaryBLL();
                    results.UserTypeName = dictBLL.Find(results.UserType).ChineseName;                
                }
            }            

            return results;
        }

        public static UserShortData ToShortViewData(this User node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            var results = new UserShortData()
            {
                Id = node.Id,
                OrganizationId = node.OrganizationId,
                Email = node.Email,
                EmailConfirmed = node.EmailConfirmed,
                //PasswordHash = node.PasswordHash,
                //SecurityStamp = node.SecurityStamp,
                PhoneNumber = node.PhoneNumber,
                PhoneNumberConfirmed = node.PhoneNumberConfirmed,
                TwoFactorEnabled = node.TwoFactorEnabled,
                LockoutEndDateUtc = node.LockoutEndDateUtc,
                LockoutEnabled = node.LockoutEnabled,
                AccessFailedCount = node.AccessFailedCount,
                UserName = node.UserName,
                FullName = node.FullName,
                IdentityNo = node.IdentityNo,
                IsResignOrGraduate = node.IsResignOrGraduate,
                IsRightInfo=node.IsRightInfo,
                ForeignId = node.ForeignId,
                EnrollDate = node.EnrollDate,
            };

            return results;
        }

        public static User ToModel(this UserData node)
        {
            return new User()
            {
                Id = node.Id,
                OrganizationId = node.OrganizationId,
                Email = node.Email,
                EmailConfirmed = node.EmailConfirmed,
                PasswordHash = node.PasswordHash,
                SecurityStamp = node.SecurityStamp,
                PhoneNumber = node.PhoneNumber,
                PhoneNumberConfirmed = node.PhoneNumberConfirmed,
                TwoFactorEnabled = node.TwoFactorEnabled,
                LockoutEndDateUtc = node.LockoutEndDateUtc,
                LockoutEnabled = node.LockoutEnabled,
                AccessFailedCount = node.AccessFailedCount,
                UserName = node.UserName,
                FullName = node.FullName,
                IdentityNo = node.IdentityNo,
                IsResignOrGraduate = node.IsResignOrGraduate,
                IsRightInfo=node.IsRightInfo,
                ForeignId = node.ForeignId,
                EnrollDate = node.EnrollDate,
                StaffNo = node.StaffNo,
                QQ = node.QQ,
                WeChat = node.WeChat,
                Gender = node.Gender ?? true,
                UserType = node.UserType,
            };
        }

        public static IEnumerable<UserData> ToViewList(this IQueryable<User> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            UserBLL userBLL = new UserBLL();
            //var results = nodeList.Select(node => new UserData()
            //{
            //    Id = node.Id,
            //    OrganizationId = node.OrganizationId,
            //    Email = node.Email,
            //    EmailConfirmed = node.EmailConfirmed,
            //    PasswordHash = node.PasswordHash,
            //    SecurityStamp = node.SecurityStamp,
            //    PhoneNumber = node.PhoneNumber,
            //    PhoneNumberConfirmed = node.PhoneNumberConfirmed,
            //    TwoFactorEnabled = node.TwoFactorEnabled,
            //    LockoutEndDateUtc = node.LockoutEndDateUtc,
            //    LockoutEnabled = node.LockoutEnabled,
            //    AccessFailedCount = node.AccessFailedCount,
            //    UserName = node.UserName,
            //    FullName = node.FullName,
            //    IdentityNo = node.IdentityNo,
            //    IsResignOrGraduate = node.IsResignOrGraduate,
            //    IsRightInfo=node.IsRightInfo,
            //    ForeignId = node.ForeignId,
            //    EnrollDate = node.EnrollDate,
            //    StaffNo = node.StaffNo,
            //    QQ = node.QQ,
            //    WeChat = node.WeChat,
            //    Gender = node.Gender,
            //    UserType = node.UserType,

            //}).ToList();
            var results = nodes.ToList().Select(x => x.ToViewData(suffix));

            UserAccountBLL userAccountBLL = new UserAccountBLL();
            foreach (var model in results)
            {
                if(model.PasswordHash!=null)
                    if (model.PasswordHash.EndsWith("_缴费账户_不登录")&&model.SecurityStamp==null)
                        model.Account = userAccountBLL.Filter(o => o.UserId == model.Id && o.Enable == true).ToViewList();
            }
            return results;
        }
    }
}
