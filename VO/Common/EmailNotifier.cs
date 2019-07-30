using EMP2016.API.VO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMP2016.API.VO.Common
{
    public class EmailNotifier:INotifier
    {

        public string Send(NotifierData msg, string userId)
        {
            DAL.EmpModels.EmpContext emp = new DAL.EmpModels.EmpContext();
            try
            {
                var u = emp.Users.FirstOrDefault(x => x.Id == userId);
                if (u.Email == null) return null;
                if (u == null) return null;
                if (msg.Msg.Body == null)
                    msg.Msg.Body = msg.Msg.Subject;
                EmailHelper e = new EmailHelper(u.Email, msg.Msg.Subject,msg.Msg.Body);
                e.Send();
                return userId;
                //}
            }
            catch (Exception ex)
            {
                MyConsole.log(ex, "邮件发送异常");
                //throw (ex);
            }
            return null;
        }

        public List<string> Send(NotifierData msg, List<string> userIds)
        {
            List<string> successes = new List<string>();
            DAL.EmpModels.EmpContext emp = new DAL.EmpModels.EmpContext();
            var users = emp.Users.Where(x => userIds.Contains(x.Id)).ToList();
            if (users.Count() == 0) return successes;

            foreach (var u in users)
            {
                try
                {
                    EmailHelper e = new EmailHelper(u.Email, msg.Msg.Subject, msg.Msg.Body);
                    e.Send();
                    successes.Add(u.Id);
                }
                catch (Exception ex)
                {
                    MyConsole.log(ex, "邮件发送异常");
                }
            }
            return successes;

        }


    }
}
