//using WxPay2017.API.DAL.EmpModels;
//using WxPay2017.API.VO.Common;
//using WxPay2017.API.VO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WxPay2017.API.BLL
//{
//    public static class TemplateExtension
//    {
//        public static TemplateData ToViewData(this Template node, CategoryDictionary suffix = CategoryDictionary.None)
//        {
//            if (node == null)
//                return null;
//            var model = new TemplateData()
//            {
//                Id = node.Id,
//                Name = node.Name,
//                Category = node.Category,
//                ApplicationId = node.ApplicationId,
//                Subject = node.Subject,
//                Body = node.Body,
//                Parameters = node.Parameters,
//                Description = node.Description,
//                CategoryDict = ((suffix & CategoryDictionary.Dictionary) == CategoryDictionary.Dictionary) ? node.CategoryDict.ToViewData() : null
//            };
//            return model;
//        }


//        public static IList<TemplateData> ToViewList(this IQueryable<Template> nodes, CategoryDictionary suffix = CategoryDictionary.None)
//        {
//            if (nodes == null)
//                return null;
//            var results = nodes.ToList().Select(x => x.ToViewData(suffix)).ToList();
//            return results;
//        }

//        public static Template ToModel(this TemplateData node)
//        {
//            return new Template()
//            {
//                Id = node.Id,
//                Name = node.Name,
//                Category = node.Category,
//                ApplicationId = node.ApplicationId,
//                Subject = node.Subject,
//                Body = node.Body,
//                Parameters = node.Parameters,
//                Description = node.Description
//            };
//        }
//    }
//}
