
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using WxPay2017.API.DAL;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
namespace WxPay2017.API.BLL
{
    public class UserBLL : Repository<User>
    {


        public UserBLL(EmpContext context = null)
            : base(context)
        {

        }

        public TargetData GetTargetInfo(string userId)
        {
            if (UserCache.TargetsByUser.Keys.Contains(userId))
                return UserCache.TargetsByUser[userId];
            
            TargetData target = new TargetData();
            User user = db.Users.FirstOrDefault(o => o.Id == userId);
            if (user == null)
                return null;
            if (user.Roles.Count(o => o.Id == RoleCache.PayMentRoleId) == 0)
                return null;
            if (user.Buildings.Count > 0)
            {
                target.Category = CategoryDictionary.Building;
                target.TargetId = user.Buildings.FirstOrDefault().Id;
                target.Name = user.Buildings.FirstOrDefault().Name;
            }
            else
            {
                target.Category = CategoryDictionary.Organization;
                target.TargetId = user.Organizations.FirstOrDefault().Id;
                target.Name = user.Organizations.FirstOrDefault().Name;
            }
            UserCache.TargetsByUser.Add(userId,target);
            return target;
        }

        /// <summary>
        /// 获得对象的付费账户
        /// </summary>
        /// <param name="category">建筑或机构</param>
        /// <param name="targetId">对象id</param>
        /// <returns></returns>
        public DAL.EmpModels.User GetAccountUser(CategoryDictionary category, int targetId, bool isfromCache = false)
        {
            if (isfromCache)
                if (UserCache.UsersByTarget.Keys.Contains(category + "_" + targetId))
                    return UserCache.UsersByTarget[category + "_" + targetId];

            User user = null;
            try
            {
                if (category == CategoryDictionary.Building)
                    user = this.db.Users.FirstOrDefault(o => o.Buildings.Any(c => c.Id == targetId) && o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId));
                else if (category == CategoryDictionary.Organization)
                    user = this.db.Users.FirstOrDefault(o =>  o.Organizations.Any(c => c.Id == targetId) && o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId));
            }
            catch
            {

            }
            try
            {
                if (user!=null)
                    UserCache.UsersByTarget.Add(category + "_" + targetId, user);
            }
            catch
            {
                if (user != null)
                    UserCache.UsersByTarget[category + "_" + targetId] = user;
            }
            return user;
        }

        /// <summary>
        /// 获取对象的收费员
        /// </summary>
        /// <param name="category"></param>
        /// <param name="targetId"></param>
        /// <param name="isfromCache"></param>
        /// <returns></returns>
        public User GetReceiver(CategoryDictionary category, int targetId, bool isfromCache = false)
        {
            if (isfromCache)
                if (UserCache.UsersByTarget.Keys.Contains(category + "_" + targetId))
                    return UserCache.UsersByTarget[category + "_" + targetId];

            User user = null;
            try
            {
                if (category == CategoryDictionary.Building)
                    user = this.db.Users.FirstOrDefault(o => o.Buildings.Any(c => c.Id == targetId) && o.Roles.Any(c => c.Id == RoleCache.TollCollectorRoleId));
                else if (category == CategoryDictionary.Organization)
                    user = this.db.Users.FirstOrDefault(o => o.Organizations.Any(c => c.Id == targetId) && o.Roles.Any(c => c.Id == RoleCache.TollCollectorRoleId));
            }
            catch
            {

            }
            try
            {
                if (user != null)
                    UserCache.UsersByTarget.Add(category + "_" + targetId, user);
            }
            catch
            {
                if (user != null)
                    UserCache.UsersByTarget[category + "_" + targetId] = user;
            }
            return user;
        }

        /// <summary>
        /// 在建筑的所属机构及其上级机构中查找最近的指定角色人员，比如维修员
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IQueryable<DAL.EmpModels.User> GetUsersInOrgTreeByRoleId(int buildingId,string roleId)
        {
            var building = db.Buildings.FirstOrDefault(o => o.Id == buildingId);
            if (building == null||building.Organization==null)
                return null;
            //获取该建筑所属的机构及其上级机构清单
            var orgIds = db.OrganizationsDbSet.Where(o => building.Organization.TreeId.StartsWith(o.TreeId)).OrderBy(o => o.TreeId.Length).Select(o => o.Id).ToList();
            foreach (var oId in orgIds)
            {
                //获取上面机构中维修员
                var Repairers = Filter(o => o.Roles.Any(c => c.Id == roleId) && o.OrganizationId != null && oId == o.OrganizationId);
                if (Repairers.Count() > 0)
                    return Repairers;
            }
            return null;
        }

        /// <summary>
        /// 简易插入用户数据，密码为123456
        /// <remarks>
        /// 返回错误信息，如正确返回null
        /// </remarks>
        /// </summary>
        /// <param name="data">用户</param>
        /// <param name="roleData">角色</param>
        /// <returns>返回错误信息，如正确返回null</returns>
        public string CreateSimpleUser(User data, 
            Role roleData)
        {
            try
            {
                
                data.AccessFailedCount = 0;
                data.EmailConfirmed = false;
                data.Id = Guid.NewGuid().ToString();
                data.IsResignOrGraduate = false;
                data.LockoutEnabled = true;
                data.PasswordHash = "AJdz9ThKYr4r/bCZfrLzzafa3UG583CDQSI7Qv7kD14JDVBFzco1RelWGBb4RDPCbA==";
                data.PhoneNumberConfirmed = false;
                data.Roles.Add(roleData);
                data.SecurityStamp = "29b57626-9bf7-41db-84d1-ba6234f096bd";
                data.TwoFactorEnabled = false;
                Create(data);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }



        #region 微信

        /// <summary>
        /// 根据微信的ID获得该用户的信息
        /// </summary>
        /// <param name="openid">微信的id</param> 
        /// <returns></returns>

        public WxUserData GetUserBlanceInfo(string openid)
        {

            DateTime beginTime = DateTime.Now.AddYears(-1);
            int num = 10; //显示近一年来的充值缴费记录
            decimal total = 0;
            string resultstr = "";
            User user = null;
            int buildingId;
            Building building = null;
            WxUserData wxuserdata = new WxUserData();

            wxuserdata.openid = openid;
            try
            {
                user = this.Filter(o => o.WeChat == openid).FirstOrDefault();
                if (user == null)
                {
                    resultstr = "该用户未绑定";
                }
                else
                {
                    wxuserdata.userid = user.Id;
                    wxuserdata.StaffNo = user.StaffNo;
                    wxuserdata.FullName = user.FullName;
                    wxuserdata.UserName = user.UserName;
                    wxuserdata.PhoneNumber = user.PhoneNumber;

                    building = user.Buildings.FirstOrDefault();
                    if (building == null)
                    {
                        resultstr = "该用户无分配房间";
                    }
                    else
                    {

                        buildingId = building.Id;
                        wxuserdata.buildingId = building.Id;
                        wxuserdata.building = building.Parent.Parent.Name;
                        wxuserdata.Level = building.Parent.Name;
                        wxuserdata.Room = building.Name;
                        user = this.GetAccountUser(CategoryDictionary.Building, buildingId);

                        if (user == null)
                        {
                            resultstr = "该用户没有缴费账户";
                        }
                        else
                        {
                            Repository<Balance> blancebll = new Repository<Balance>();
                            Repository<HistoryBill> historybillbLL = new Repository<HistoryBill>();
                            total = blancebll.Filter(o => o.TargetCategory == (int)CategoryDictionary.Building && o.TargetId == buildingId).OrderByDescending(o => o.Id).FirstOrDefault().Total;
                            wxuserdata.Balance = total;



                            ////获得最近10条缴费记录
                            wxuserdata.Feelist = historybillbLL.Filter(o =>
                                o.ReceiverId == user.Id
                                && o.IsPay == true
                                &&
                                ((o.BillTypeId == 120003  // 缴费账单
                                && o.PayMethodId <= 370005   //预交费记录
                                ) || (o.BillTypeId == 120006))  // 现金充值
                                && o.PayMentTime > beginTime
                                ).OrderByDescending(o => o.PayMentTime).Take(num).Select(s => new WxUserFee
                                {
                                    FeeAmount = s.Value,
                                    Feedata = s.PayMentTime,
                                     IsSynchro=s.IsSynchro
                                }).ToList();

                            //wxuserdata.Feelist = historybillbLL.Filter(o =>
                            //      o.ReceiverId == user.Id
                            //      && o.IsPay == true
                            //      &&
                            //      ((o.BillTypeId == DictionaryCache.BillTypePay.Id  // 缴费账单
                            //      && o.PayMethodId <= DictionaryCache.PayMethodCash.Id   //预交费记录
                            //      ) || (o.BillTypeId == DictionaryCache.BillTypePrePay.Id))  // 现金充值
                            //      && o.PayMentTime > beginTime
                            //      ).OrderByDescending(o => o.PayMentTime).Take(num).Select(s => new WxUserFee
                            //      {
                            //          FeeAmount = s.Value,
                            //          Feedata = s.PayMentTime
                            //      }).ToList();





                            blancebll.Dispose();
                            historybillbLL.Dispose();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                resultstr = "数据加载出错，请重新打开";
            }

            wxuserdata.Result = resultstr;


            return wxuserdata;

        }

        public string UpdateOpenid(string phone, string stuno, string openid)
        {

            User user = null;
            int k = 0;
            try
            {
                user = this.Filter(o => o.PhoneNumber == phone && o.StaffNo == stuno).FirstOrDefault();
                user.WeChat = openid;
                k = this.Update(user);
            }
            catch
            {

            }


            return k > 0 ? "OK" : "Fail";
        }


        #endregion









    }
}