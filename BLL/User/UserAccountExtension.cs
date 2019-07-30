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
   public  static class UserAccountExtension
    {
        #region UserAccount
       static Encrypt encrypt = new Encrypt();
        public static UserAccountData ToViewData(this UserAccount node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            UserBLL userBLL = new UserBLL();
            return new UserAccountData()
            {
                Id = node.Id,
                UserId = node.UserId,
                Balance =encrypt.Decrypto( node.Balance),
                BalanceTypeId = node.BalanceTypeId,
                AddTime = node.AddTime,
                Enable = node.Enable,
                BalanceTypeName = node.BalanceType == null ? DictionaryCache.Get()[node.BalanceTypeId].ChineseName : node.BalanceType.ChineseName,
                User = (suffix & CategoryDictionary.User) == CategoryDictionary.User ?(node.User==null?userBLL.Find(node.UserId).ToViewData():node.User.ToViewData()) : null
            };
        }

        public static IList<UserAccountData> ToViewList(this IQueryable<UserAccount> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            UserBLL userBLL = new UserBLL();
            var results = nodeList.Select(node => new UserAccountData()
            {
                Id = node.Id,
                UserId = node.UserId,
                Balance = encrypt.Decrypto(node.Balance),
                BalanceTypeId = node.BalanceTypeId,
                AddTime = node.AddTime,
                Enable = node.Enable,
                BalanceTypeName = node.BalanceType == null ? DictionaryCache.Get()[node.BalanceTypeId].ChineseName : node.BalanceType.ChineseName,
                User = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? (node.User == null ? userBLL.Find(node.UserId).ToViewData() : node.User.ToViewData()) : null

            }).ToList();
            return results;
        }

        public static UserAccount ToModel(this UserAccountData node)
        {
            return new UserAccount()
            {
                Id = node.Id,
                UserId = node.UserId,
                Balance =encrypt.Encrypto( node.Balance),
                BalanceTypeId = node.BalanceTypeId,
                AddTime = node.AddTime,
                Enable = node.Enable,


            };
        }
        #endregion

    }
}
