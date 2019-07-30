using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public class BalanceDetailBLL : Repository<BalanceDetail>
    {
        public DictionaryBLL dictionaryBLL = new DictionaryBLL();

        public BuildingBLL buildingBLL = new BuildingBLL();
        public MeterBLL meterBLL = new MeterBLL();
        public OrganizationBLL organizationBLL = new OrganizationBLL();
        public MonitoringConfigBLL monitoringConfigBLL = new MonitoringConfigBLL();
        public UserBLL userBLL = new UserBLL();


        public BalanceDetailBLL(EmpContext context = null)
            : base(context)
        {
            dictionaryBLL = new DictionaryBLL(this.db);
            buildingBLL = new BuildingBLL(this.db);
            organizationBLL = new OrganizationBLL(this.db);
            monitoringConfigBLL = new MonitoringConfigBLL(this.db);
            meterBLL = new MeterBLL(this.db);
            userBLL = new UserBLL(this.db);
        }

    }
}
