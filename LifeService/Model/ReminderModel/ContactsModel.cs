using System;
using System.Data;

namespace LifeService.Model.ReminderModel
{
    /// <summary>
    /// 联系人模型
    /// </summary>
    internal class ContactsModel:IDataModel
    {
        //创建
        public static string CreateSql =
            "create table contacts (name varchar2(32) , phonenumber varchar2(20) primary key)";

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string InsertSql()
        {
            throw new NotImplementedException();
        }

        public string UpdateSql()
        {
            throw new NotImplementedException();
        }

        public string DeleteSql()
        {
            throw new NotImplementedException();
        }

        public string SelectSql()
        {
            return "select * from contacts";
        }

        public void FetchData(DataRow data)
        {
            Name = data["Name"].ToString();
            PhoneNumber = data["PhoneNumber"].ToString();
        }
    }
}
