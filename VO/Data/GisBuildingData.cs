using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class GisBuildingData
    {

        public GisBuildingData()
        {
            Children = new List<GisBuildingData>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public int? Year { get; set; }

        public int? UpFloor { get; set; }

        public int Sort { get; set; }

        public decimal? Electric { get; set; }

        public decimal? Water { get; set; }

        public decimal? ElectricYear { get; set; }

        public decimal? WaterYear { get; set; }

        public decimal? ElectricYesterday { get; set; }

        public decimal? WaterYesterday { get; set; }

        public decimal? ElectricToday { get; set; }

        public decimal? WaterToday { get; set; }

        #region Heating
        public decimal? Heating { get; set; }
        public decimal? HeatingYear { get; set; }
        public decimal? HeatingYesterday { get; set; }
        public decimal? HeatingToday { get; set; }
        #endregion

        public string Icon { get; set; }

        public decimal? TotalArea { get; set; }

        public decimal? WorkingArea { get; set; }

        public decimal? LivingArea { get; set; }

        public decimal? ReceptionArea { get; set; }

        public int? ManagerCount { get; set; }

        public decimal? CustomerCount { get; set; }

        public string BuildingCategoryName { get; set; }

        public int? OrganizationId { get; set; }

        public List<GisBuildingData> Children { get; set; }

        public virtual ICollection<MeterData> Meters { get; set; }

        public virtual CoordinateData Coordinate2d { get; set; }

        public virtual CoordinateData Coordinate3d { get; set; }

        public virtual CoordinateData CoordinateMap { get; set; }
    }
}
