using System;
using System.Data;

namespace LifeService.Model.WeatherModel
{
    /// <summary>
    /// 天气模型
    /// </summary>
    internal class WeatherModel:IDataModel
    {
        public static string CreateCitySql =
            "create table weathercity (guid varchar2(32) primary key, city varchar2(32),degreeformat varchar2(1))";

        /// <summary>
        /// 唯一值 
        /// </summary>
        public string GuiD { get; set; }

        /// <summary>
        /// 城市名
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 天气格式
        /// </summary>
        public string DegreeFormat { get; set; }

        public string InsertSql()
        {
            return String.Format("insert into weathercity values('{0}','{1}','{2}')", GuiD, City, DegreeFormat);
        }

        public string UpdateSql()
        {
            return String.Format("update weathercity set City='{0}',DegreeFormat='{1}'", City,
                DegreeFormat);
        }

        public string DeleteSql()
        {
            throw new NotImplementedException();
        }

        public string SelectSql()
        {
            return "select * from weathercity";
        }

        public void FetchData(DataRow data)
        {
            GuiD = data["GuiD"].ToString();
            City = data["City"].ToString();
            DegreeFormat = data["DegreeFormat"].ToString();
        }
    }
}
