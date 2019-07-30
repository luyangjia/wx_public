using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;

namespace WxPay2017.API.BLL
{
    public class UserAccountBLL : Repository<UserAccount>
    {
        private UserBLL userBLL = new UserBLL();
        private BuildingBLL buildingBLL = new BuildingBLL();
        private OrganizationBLL organizationBLL = new OrganizationBLL();
        private DictionaryBLL dictionaryBLL = new DictionaryBLL();
        private RoleBLL roleBLL = new RoleBLL();

        public UserAccountBLL(EmpContext context = null)
            : base(context)
        {
            userBLL = new UserBLL(this.db);
            buildingBLL = new BuildingBLL(this.db);
            organizationBLL = new OrganizationBLL(this.db);
            dictionaryBLL = new DictionaryBLL(this.db);
            roleBLL = new RoleBLL(this.db);
        }

        /// <summary>
        /// 根据配置创建用户账户，缴费角色
        /// </summary>
        /// <param name="monitoringConfig"></param>
        /// <param name="node"></param>
        public void CreateAcountByConfig(MonitoringConfig node)
        {
            //定价或补贴需要缴费账户
            if (node.ConfigTypeId == DictionaryCache.MonitoringConfigTypePrice.Id || node.ConfigTypeId == DictionaryCache.MonitoringConfigTypeSubsidy.Id)
            {
                //是否已经存在缴费账户
                int count = 0;

                //为当前对象开启缴费账户
                CategoryDictionary category = CategoryDictionary.Building;
                if (node.TargetTypeId == DictionaryCache.ConfigToOrg.Id)
                {
                    category = CategoryDictionary.Organization;
                    count = userBLL.Count(o => o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId) && o.Organizations.Any(c => c.Id == node.TargetId));
                }
                else
                {
                    count = userBLL.Count(o => o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId) && o.Buildings.Any(c => c.Id == node.TargetId));
                }
                if (count > 0)
                    return;
               
                string error = "";
                List<UserAccountData> userAccounts;
                CreateAccounts(category, true, new List<string> { node.TargetId + "" }, out error, out userAccounts);
            }
        }

        /// <summary>
        /// 批量创建缴费账号
        /// </summary>
        /// <param name="category">用户，机构或建筑</param>
        /// <param name="isOnlyOne">是否只创建一个统一管理账户（对建筑，组织机构）</param>
        /// <param name="ids">对象id集合</param>
        /// <param name="error">返回的错误信息</param>
        /// <param name="userAccounts">创建的账号</param>
        public void CreateAccounts(CategoryDictionary category, bool isOnlyOne, List<string> ids, out string error, out List<UserAccountData> userAccounts,bool isSaveChange=true)
        {

            Encrypt encrypt = new Encrypt();
            error = "";
            userAccounts = new List<UserAccountData>();
            var roleId =  RoleCache.PayMentRoleId;
            var role = this.db.Roles.FirstOrDefault(o => o.Id == roleId);
            if (isSaveChange != true)
            {
                userBLL.shareContext = true;
            }
            //var balanceTypes = DictionaryCache.Get().Values.Where(o => o.Code == "BalanceType" && o.Enable == true).ToList();
            switch (category)
            {

                case CategoryDictionary.Building:
                    if (isOnlyOne == false)
                        foreach (var idStr in ids)
                        {
                            try
                            {
                                int id = Convert.ToInt32(idStr);
                                var building = buildingBLL.Find(id);
                                if (building == null)
                                    continue;
                                var user = userBLL.Filter(o => o.Roles.Any(c => c.Id == roleId) && o.Buildings.Any(c => c.Id == building.Id)).ToList();
                                if (user.Count() == 0)
                                {
                                    //不存在，创建
                                    User u = new User();
                                    u.Id = (Guid.NewGuid().ToString("D"));
                                    u.UserName = building.Id + "";
                                    u.FullName = building.Name;
                                    u.PasswordHash = "fjnewcap_缴费账户_不登录";
                                    u.EmailConfirmed = true;
                                    u.PhoneNumberConfirmed = true;
                                    u.TwoFactorEnabled = true;
                                    u.StaffNo = "";
                                    u.LockoutEnabled = false;
                                    u.AccessFailedCount = 0;
                                    u.Roles.Add(role);
                                    u.IsResignOrGraduate = false;
                                    u.EnrollDate = DateTime.Now;
                                    u.Buildings.Add(building);
                                    userBLL.Create(u);

                                    //foreach (var item in balanceTypes)
                                    //{
                                    //    var account = new UserAccount();
                                    //    account.AddTime = DateTime.Now;
                                    //    account.Balance = encrypt.Encrypto("0");
                                    //    account.UserId = u.Id;
                                    //    account.Enable = true;
                                    //    account.BalanceTypeId = item.Id;
                                    //    account = this.Create(account);
                                    //    userAccounts.Add(account.ToViewData());
                                    //}
                                }
                                //else
                                //{
                                //    //存在，开启
                                //    string uid = user[0].Id;
                                //    var accounts = this.Filter(o => o.UserId == uid).ToList();
                                //    foreach (var item in accounts)
                                //    {
                                //        item.Enable = true;
                                //        this.Update(item);
                                //        userAccounts.Add(item.ToViewData());
                                //    }
                                //}
                            }
                            catch { }
                        }
                    else
                    {
                        try
                        {
                            var user = userBLL.Filter(o => o.Roles.Any(c => c.Id == roleId) && o.Buildings.Any(c => ids.Contains(c.Id + ""))).ToList();
                            if (user.Count() != 0)
                            {
                                error = "已经有建筑存在缴费账户，请使用添加建筑至现有缴费账户操作实现";
                            }
                            else
                            {
                                //不存在，创建
                                User u = new User();
                                u.Id = (Guid.NewGuid().ToString("D"));
                                u.UserName = "公用缴费账户";
                                u.FullName = "公用缴费账户";
                                u.PasswordHash = "fjnewcap_缴费账户_不登录";
                                u.EmailConfirmed = true;
                                u.PhoneNumberConfirmed = true;
                                u.TwoFactorEnabled = true;
                                u.LockoutEnabled = false;
                                u.StaffNo = "";
                                u.AccessFailedCount = 0;
                                u.IsResignOrGraduate = false;
                                u.EnrollDate = DateTime.Now;
                                u.Roles.Add(role);
                                foreach (var id in ids)
                                {
                                    try
                                    {
                                        int buildingId = Convert.ToInt32(id);

                                        var users = userBLL.Filter(o => o.Roles.Any(c => c.Id == roleId) && o.Buildings.Any(c => c.Id == buildingId)).ToList();
                                        foreach (var user1 in users)
                                        {
                                            var builiding = user1.Buildings.FirstOrDefault(o => o.Id == buildingId);
                                            user1.Buildings.Remove(builiding);
                                        }
                                        u.Buildings.Add(buildingBLL.Find(buildingId));
                                    }
                                    catch { }
                                }
                                userBLL.Create(u);

                                //foreach (var item in balanceTypes)
                                //{
                                //    var account = new UserAccount();
                                //    account.AddTime = DateTime.Now;
                                //    account.Balance = encrypt.Encrypto("0");
                                //    account.UserId = u.Id;
                                //    account.Enable = true;
                                //    account.BalanceTypeId = item.Id;
                                //    account = this.Create(account);
                                //    userAccounts.Add(account.ToViewData());
                                //}
                            }
                        }
                        catch { }
                    }
                    break;

                case CategoryDictionary.Organization:
                    if (isOnlyOne == false)
                        foreach (var idStr in ids)
                        {
                            try
                            {
                                int id = Convert.ToInt32(idStr);
                                var organization = organizationBLL.Find(id);
                                if (organization == null)
                                    continue;
                                var user = userBLL.Filter(o => o.Roles.Any(c => c.Id == roleId) && o.Organizations.Any(c => c.Id == organization.Id)).ToList();
                                if (user.Count() == 0)
                                {
                                    //不存在，创建
                                    User u = new User();
                                    u.Id = (Guid.NewGuid().ToString("D"));
                                    u.UserName = organization.Id + "";
                                    u.FullName = organization.Name;
                                    u.PasswordHash = "fjnewcap_缴费账户_不登录";
                                    u.EmailConfirmed = true;
                                    u.PhoneNumberConfirmed = true;
                                    u.TwoFactorEnabled = true;
                                    u.LockoutEnabled = false;
                                    u.StaffNo = "";
                                    u.AccessFailedCount = 0;
                                    u.IsResignOrGraduate = false;
                                    u.EnrollDate = DateTime.Now;
                                    u.Roles.Add(role);
                                    u.Organizations.Add(organization);
                                    userBLL.Create(u);
                                    //foreach (var item in balanceTypes)
                                    //{
                                    //    var account = new UserAccount();
                                    //    account.AddTime = DateTime.Now;
                                    //    account.Balance = encrypt.Encrypto("0");
                                    //    account.UserId = u.Id;
                                    //    account.Enable = true;
                                    //    account.BalanceTypeId = item.Id;
                                    //    account = this.Create(account);
                                    //    userAccounts.Add(account.ToViewData());
                                    //}
                                }
                                //else
                                //{
                                //    //存在，开启
                                //    string uid = user[0].Id;
                                //    var accounts = this.Filter(o => o.UserId == uid).ToList();
                                //    foreach (var item in accounts)
                                //    {
                                //        item.Enable = true;
                                //        this.Update(item);
                                //        userAccounts.Add(item.ToViewData());
                                //    }
                                //}
                            }
                            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                            {

                                throw dbEx;
                            }
                            catch { }
                        }
                    else
                    {
                        var user = userBLL.Filter(o => o.Roles.Any(c => c.Id == roleId) && o.Organizations.Any(c => ids.Contains(c.Id + ""))).ToList();
                        if (user.Count() != 0)
                        {
                            error = "已经有组织机构存在缴费账户，请使用添加组织机构至现有缴费账户操作实现";
                        }
                        else
                        {
                            //不存在，创建
                            User u = new User();
                            u.Id = (Guid.NewGuid().ToString("D"));
                            u.UserName = "公用缴费账户";
                            u.FullName = "公用缴费账户";
                            u.PasswordHash = "fjnewcap_缴费账户_不登录";
                            u.EmailConfirmed = true;
                            u.PhoneNumberConfirmed = true;
                            u.TwoFactorEnabled = true;
                            u.LockoutEnabled = false;
                            u.IsResignOrGraduate = false;
                            u.StaffNo = "";
                            u.EnrollDate = DateTime.Now;
                            u.AccessFailedCount = 0;
                            u.Roles.Add(role);
                            foreach (var id in ids)
                            {
                                try
                                {
                                    u.Organizations.Add(organizationBLL.Find(Convert.ToInt32(id)));
                                }
                                catch { }
                            }
                            userBLL.Create(u);

                            //foreach (var item in balanceTypes)
                            //{
                            //    var account = new UserAccount();
                            //    account.AddTime = DateTime.Now;
                            //    account.Balance = encrypt.Encrypto("0");
                            //    account.UserId = u.Id;
                            //    account.Enable = true;
                            //    account.BalanceTypeId = item.Id;
                            //    account = this.Create(account);
                            //    userAccounts.Add(account.ToViewData());
                            //}
                        }
                    }
                    break;

                case CategoryDictionary.User:
                    foreach (var id in ids)
                    {
                        try
                        {
                            var user = userBLL.Find(id);
                            if (user == null)
                                continue;
                            var accounts = this.Filter(o => o.UserId == user.Id).ToList();
                            if (accounts.Count() == 0)
                            {
                                //add
                                //foreach (var item in balanceTypes)
                                //{
                                //    var account = new UserAccount();
                                //    account.AddTime = DateTime.Now;
                                //    account.Balance = encrypt.Encrypto("0");
                                //    account.UserId = user.Id;
                                //    account.Enable = true;
                                //    account.BalanceTypeId = item.Id;
                                //    account = this.Create(account);
                                //    userAccounts.Add(account.ToViewData());
                                //}
                            }
                            //else
                            //{
                            //    //已经存在，则开启
                            //    foreach (var item in accounts)
                            //    {
                            //        item.Enable = true;
                            //        this.Update(item);
                            //        userAccounts.Add(item.ToViewData());
                            //    }
                            //}
                        }
                        catch { }
                    }
                    break;

                default:
                    error = "只为用户、机构、建筑开启账户";
                    break;
            }
        }

    }
}
