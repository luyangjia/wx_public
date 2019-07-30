using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;
namespace WxPay2017.API.VO.Common
{
    public static class RoleCache
    {
        public static string PayMentRoleId { get; set; }
        public static Role PayMentRole { get; set; }
        public static string RepairerRoleId { get; set; }
        public static string RepairManagerRoleId { get; set; }
        public static string AdminRoleId { get; set; }
        public static string PowerUserRoleId { get; set; }
        public static string TollCollectorRoleId { get; set; }

        public static void Init()
        {
            DAL.EmpModels.EmpContext db = new DAL.EmpModels.EmpContext();

            if (db.Roles.Count(o => o.Name == "缴费账户") == 0)
            {
                Role role = new Role();
                role.Name = "缴费账户";
                Guid guid = Guid.NewGuid();
                role.Id = guid.ToString();
                db.Roles.Add(role);
            }
            //if (db.Roles.Count(o => o.Name == "维修员") == 0)
            //{
            //    Role role = new Role();
            //    role.Name = "维修员";
            //    Guid guid = Guid.NewGuid();
            //    role.Id = guid.ToString();
            //    db.Roles.Add(role);
            //}
            //if (db.Roles.Count(o => o.Name == "维修管理员") == 0)
            //{
            //    Role role = new Role();
            //    role.Name = "维修管理员";
            //    Guid guid = Guid.NewGuid();
            //    role.Id = guid.ToString();
            //    db.Roles.Add(role);
            //}
            if (db.Roles.Count(o => o.Name == "能耗用户") == 0)
            {
                Role role = new Role();
                role.Name = "能耗用户";
                Guid guid = Guid.NewGuid();
                role.Id = guid.ToString();
                db.Roles.Add(role);
            }
            if (db.Roles.Count(o => o.Name == "收费员") == 0)
            {
                Role role = new Role();
                role.Name = "收费员";
                Guid guid = Guid.NewGuid();
                role.Id = "2";
                db.Roles.Add(role);
            }
            db.SaveChanges();
            var roles = db.Roles.ToList();
            PayMentRoleId = roles.FirstOrDefault(o => o.Name == "缴费账户").Id;
            PayMentRole = roles.FirstOrDefault(o => o.Name == "缴费账户");
            //RepairerRoleId = roles.FirstOrDefault(o => o.Name == "维修员").Id;
            //RepairManagerRoleId = roles.FirstOrDefault(o => o.Name == "维修管理员").Id;
            AdminRoleId = roles.FirstOrDefault(o => o.Name == "超级管理员").Id;
            PowerUserRoleId = roles.FirstOrDefault(o => o.Name == "能耗用户").Id;
            TollCollectorRoleId = roles.FirstOrDefault(o => o.Name == "收费员").Id;
        }
    }
}
