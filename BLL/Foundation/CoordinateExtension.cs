using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class CoordinateExtension
    {
        #region Corrdinate
        public static CoordinateData ToViewData(this Coordinate node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new CoordinateData()
            {
                Id = node.Id,
                Name = node.Name,
                Type = node.Type,
                X = node.X,
                Y = node.Y,
                Points = node.Points
            };
        }

        public static IEnumerable<CoordinateData> ToViewList(this IQueryable<Coordinate> node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return node.ToList().Select(x => x.ToViewData(suffix));
        }

        public static Coordinate ToModel(this CoordinateData node)
        {
            return new Coordinate()
            {
                Id = node.Id,
                Name = node.Name,
                Type = node.Type,
                X = node.X,
                Y = node.Y,
                Points = node.Points
            };
        }
        #endregion
    }
}
