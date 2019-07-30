using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public class ParameterBLL : Repository<Parameter>
    {
         
        public ParameterBLL(EmpContext context = null)
            : base(context)
        {
        }

        public Parameter Get(int id)
        {
            return db.Parameters.FirstOrDefault(p => p.Id == id);
        }
    }
}
