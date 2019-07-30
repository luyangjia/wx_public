
namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;

    public partial class TemplateData
    { 
        public long Id { get; set; }
         
        public string Name { get; set; }

        public int Category { get; set; }

        public Guid? ApplicationId { get; set; }
         
        public string Subject { get; set; }
         
        public string Body { get; set; }
         
        public string Parameters { get; set; }
         
        public string Description { get; set; }

        public virtual DictionaryData CategoryDict { get; set; }
       

    }
     
}
