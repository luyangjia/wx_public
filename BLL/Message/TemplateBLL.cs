//using WxPay2017.API.DAL.EmpModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Reflection;
//using WxPay2017.API.VO.Common;
//using WxPay2017.API.VO.Param;
//using WxPay2017.API.VO;
//namespace WxPay2017.API.BLL
//{
//    public class TemplateBLL : Repository<Template>
//    {
//        private UserBLL userBLL = new UserBLL();
//        private SubscribeBLL subscribeBLL = new SubscribeBLL();
//        private BuildingBLL buildingBLL = new BuildingBLL();
//        private OrganizationBLL organizationBLL = new OrganizationBLL();
//        private MessageRecordBLL messageRecordBLL = new MessageRecordBLL();


//        public TemplateBLL(EmpContext context = null)
//            : base(context)
//        { 
//            userBLL = new UserBLL(this.db);
//            subscribeBLL = new SubscribeBLL(this.db);
//            userBLL = new UserBLL(this.db);
//            subscribeBLL = new SubscribeBLL(this.db);
//            buildingBLL = new BuildingBLL(this.db);
//            organizationBLL = new OrganizationBLL(this.db);
//            messageRecordBLL = new MessageRecordBLL(this.db);
//        }

//    }
//}
