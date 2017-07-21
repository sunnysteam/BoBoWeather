using System.Data;

namespace LifeService.Model
{
    /// <summary>
    /// 数据模型接口
    /// </summary>
    internal interface IDataModel
    {
        string InsertSql();
        string UpdateSql();
        string DeleteSql();
        string SelectSql();
        void FetchData(DataRow data);
    }
}
