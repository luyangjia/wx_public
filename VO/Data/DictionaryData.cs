using System.Collections.Generic;
namespace WxPay2017.API.VO
{

    public partial class DictionaryData
    {
        public int Id { get; set; }

        public string TreeId { get; set; }

        public string Code { get; set; }

        public string GbCode { get; set; }

        public int? FirstValue { get; set; }

        public int? SecondValue { get; set; }

        public int? ThirdValue { get; set; }

        public int? FourthValue { get; set; }

        public int? FifthValue { get; set; }

        public string ChineseName { get; set; }

        public string EnglishName { get; set; }

        public bool Enable { get; set; }

        public bool IsTypeOfStatisticalAnalysis { get; set; }

        public string Description { get; set; }

        public decimal? EquValue { get; set; }

        public string EquText { get; set; }

        public DictionaryData Parent { get; set; }

        public virtual ICollection<DictionaryData> Parameters { get; set; }
    }
}
