﻿using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public class ExtensionFieldBLL : Repository<ExtensionField>
    {

        public ExtensionFieldBLL(EmpContext context = null)
            : base(context)
        {
        }
    }
}
