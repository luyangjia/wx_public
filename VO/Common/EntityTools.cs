using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Common
{
   public  class EntityTools
    {
        /// <summary>

        ///用","号分隔需要操作的属性

        /// </summary>

        /// <param name="source">源对象</param>

        /// <param name="target">目标对象</param>

        /// <param name="ignorePoperties"></param>

        /// <returns></returns>

        public static TTarget EntityCopyForUpdate<TSource, TTarget>(TSource source, TTarget target, string IgnorePoperties)
        {
            List<string> ignoreP = new List<string>();

            if (!string.IsNullOrEmpty(IgnorePoperties))
            {

                ignoreP = IgnorePoperties.ToLower().Split(',').ToList();

            }


            var tFields = target.GetType().GetProperties();

            var sFields = source.GetType().GetProperties();



            foreach (var item in tFields)
            {

                if (!ignoreP.Contains(item.Name.ToLower()))
                {

                    foreach (var si in sFields)
                    {

                        if (si.Name == item.Name)
                        {

                            object svalue = si.GetValue(source, null);

                            object tvalue = item.GetValue(target, null);

                            if (svalue != null && !svalue.Equals(tvalue) && svalue.ToString()!="0")
                            {
                                if (!svalue.GetType().Name.ToLower().Contains("datetime"))
                                    item.SetValue(target, svalue, null);
                                else
                                {
                                    DateTime dt = Convert.ToDateTime(svalue);
                                    if (dt.Year != 1)
                                    {
                                        item.SetValue(target, svalue, null);
                                    }
                                }

                            }

                        }

                    }

                }

            }

            return target;



        }
        public static List<TSource> EntitySetNull<TSource>(List<TSource> sources, string notIgnorePoperties)
        {
            List<string> ignoreP = new List<string>();

            if (!string.IsNullOrEmpty(notIgnorePoperties))
            {

                ignoreP = notIgnorePoperties.ToLower().Split(',').ToList();

            }



            var tFields = sources[0].GetType().GetProperties();



            foreach (var source in sources)
            {
                foreach (var item in tFields)
                {

                    if (!ignoreP.Contains(item.Name.ToLower()))
                    {
                        try
                        {
                            item.SetValue(source, null, null);
                        }
                        catch { }
                        {

                        }

                    }
                }

            }

            return sources;



        }

        public static TSource EntitySetNull<TSource>(TSource sources, string notIgnorePoperties)
        {
            List<string> ignoreP = new List<string>();

            if (!string.IsNullOrEmpty(notIgnorePoperties))
            {

                ignoreP = notIgnorePoperties.ToLower().Split(',').ToList();

            }

            var tFields = sources.GetType().GetProperties();




            {
                foreach (var item in tFields)
                {

                    if (!ignoreP.Contains(item.Name.ToLower()))
                    {
                        try
                        {
                            item.SetValue(sources, null, null);
                        }
                        catch { }
                        {

                        }

                    }
                }

            }

            return sources;



        }
        /// <summary>

        ///用","号分隔忽略属性

        /// </summary>

        /// <param name="source">源对象</param>

        /// <param name="target">目标对象</param>

        /// <param name="ignorePoperties"></param>

        /// <returns></returns>

        public static TTarget EntityCopy<TSource, TTarget>(TSource source, TTarget target, string ignorePoperties)
        {



            List<string> ignoreP = new List<string>();

            if (!string.IsNullOrEmpty(ignorePoperties))
            {

                ignoreP = ignorePoperties.ToLower().Split(',').ToList();

            }



            ignoreP.Add("entitykey");

            ignoreP.Add("entitystate");

            var tFields = target.GetType().GetProperties();

            var sFields = source.GetType().GetProperties();



            foreach (var item in tFields)
            {

                if (!ignoreP.Contains(item.Name.ToLower()))
                {

                    foreach (var si in sFields)
                    {

                        if (si.Name == item.Name)
                        {

                            object svalue = si.GetValue(source, null);

                            object tvalue = item.GetValue(target, null);

                            if (svalue != null && !svalue.Equals(tvalue))
                            {

                                item.SetValue(target, svalue, null);

                            }

                        }

                    }

                }

            }
            return target;
        }
    }
}
