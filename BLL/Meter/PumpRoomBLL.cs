﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;

namespace WxPay2017.API.BLL
{
    public class PumpRoomBLL : Repository<PumpRoom>
    {

        public PumpRoomBLL(EmpContext context = null)
            : base(context)
        {
        }

    }
}
