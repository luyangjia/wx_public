﻿using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public class StatisticsSubscribeBLL : Repository<StatisticsSubscribe>
    {

        public StatisticsSubscribeBLL(EmpContext context = null)
            : base(context)
        {
        }

    }


}