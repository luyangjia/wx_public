using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WxPay2017.API.VO.Param;

namespace WxPay2017.API.BLL
{
    public class SettingBLL : Repository<Setting>
    { 


        public SettingBLL(EmpContext context = null)
            : base(context)
        { 
        }
        public Setting GetSetting()
        {
            string appId = MyConsole.GetAppString("ApplicationID");
            return this.Filter(o => o.AppID.ToString() == appId).FirstOrDefault();
        }

        public void InitSetting()
        {
            string appId = MyConsole.GetAppString("ApplicationID");
            if (appId != null)
            {
                var setting = this.Filter(o => o.AppID.ToString() == appId).FirstOrDefault();
                if (setting == null)
                {
                    setting = new Setting();
                    //setting.AppID = Guid.NewGuid();
                    setting.AppID = new Guid(appId);
                    //setting.Title = MyConsole.GetAppString("TopOrgName");
                    setting.Title = "初始院校";
                    setting.Data = "";
                    setting.Weather = "";
                    setting.ElectricityPrice = 0;
                    setting.WaterPrice = 0;
                    setting.ElectricityPrePay = 0;
                    setting.WaterPrePay = 0;
                    setting = this.Create(setting);
                }
            }
        }
    }
}