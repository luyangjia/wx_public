using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WxPay2017.API.VO
{
    public partial class FeedbackData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string HandleUserId { get; set; }
        public string HandleReply { get; set; }
        public DateTime? HandleTime { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public int StateId { get; set; }

        public string StateName { get; set; }
        public string TypeName { get; set; }
       // public UserData HandleUser { get; set; }
        public UserData User { get; set; }
        public UserData HandleUser { get; set; }

        public DictionaryData State { get; set; }
        public bool IsDeleted { get; set; }
    }
}
