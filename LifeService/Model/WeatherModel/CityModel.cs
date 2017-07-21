using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeService.Model.WeatherModel
{
    internal class CityModel : IDataModel
    {
        public string DeleteSql()
        {
            throw new NotImplementedException();
        }

        public void FetchData(DataRow data)
        {
            
        }

        public string InsertSql()
        {
            throw new NotImplementedException();
        }

        public string SelectSql()
        {
            return "select t.cityname from city t";
        }

        public string UpdateSql()
        {
            throw new NotImplementedException();
        }
    }
}
