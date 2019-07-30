using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public class DictionaryBLL : Repository<Dictionary>
    { 
        public DictionaryBLL(EmpContext context = null): base(context)
        { 
        }

        public List<Dictionary> GetDescents(int id, bool includeSelf = false)
        {
            var model = SystemInfo.AllDictStatus.Find(o => o.Id == id);
            if (model == null) return new List<Dictionary>();
            return SystemInfo.AllDictStatus.Find(o => o.Id == id).Descendants(includeSelf).ToList();
        }

        public Dictionary Get(int id)
        {
            return this.Find(id);
        }
        public IQueryable<Dictionary> Get(List<int> id)
        {
            return this.Filter(d => id.Contains(d.Id));
        }

        public Task<List<int>> GetDescendantsId(int categoryId, bool includeSelf = false)
        {
            return Task.Run(() =>
            {
                var model = SystemInfo.AllDictStatus.Find(o => o.Id == categoryId);
                if (model == null) return new List<int>();
                return model.Descendants(true).Select(d => d.Id).ToList();
            });

        }
    }

}
