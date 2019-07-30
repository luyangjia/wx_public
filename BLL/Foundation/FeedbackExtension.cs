using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;
using WxPay2017.API.DAL.EmpModels;


namespace WxPay2017.API.BLL
{
    public static class FeedbackExtension
    {


        public static FeedbackData ToViewData(this Feedback node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new FeedbackData()
            {
                Id = node.Id,
                UserId = node.UserId,
                TypeId = node.TypeId,
                Description = node.Description,
                CreateTime = node.CreateTime,
                HandleUserId = node.HandleUserId,
                HandleTime = node.HandleTime,
                HandleReply = node.HandleReply,
                Rating = node.Rating,
                Comment = node.Comment,
                StateId = node.StateId,
                StateName = DictionaryCache.Get()[node.StateId].ChineseName,
                TypeName = DictionaryCache.Get()[node.TypeId].ChineseName,
                IsDeleted = node.IsDeleted,
                User = ((suffix & CategoryDictionary.User) == CategoryDictionary.User) && node.HandleUser != null ? node.User.ToViewData() : null,
                HandleUser = ((suffix & CategoryDictionary.Manager) == CategoryDictionary.Manager) && node.HandleUser != null ? node.HandleUser.ToViewData() : null,
                State = node.State.ToViewData()
            };
        }

        public static IList<FeedbackData> ToViewList(this IQueryable<Feedback> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var results = nodes.ToList().Select(x => x.ToViewData(suffix)).ToList();
            return results;
        }

        public static Feedback ToModel(this FeedbackData node)
        {
            return new Feedback()
            {
                Id = node.Id,
                UserId = node.UserId,
                TypeId = node.TypeId,
                Description = node.Description,
                CreateTime = (node.CreateTime == DateTime.Parse("0001/1/1 0:00:00") || (node.CreateTime <= DateTime.Parse("2000/1/1 0:00:00"))) ? DateTime.Now : node.CreateTime, //? (node.CreateTime == null ? DateTime.Now : node.CreateTime) : node.CreateTime,
                HandleUserId = node.HandleUserId,
                HandleTime = node.HandleTime,
                HandleReply = node.HandleReply,
                Rating = node.Rating,
                Comment = node.Comment,
                StateId = node.StateId,
                IsDeleted = node.IsDeleted,


            };
        }

    }
}
