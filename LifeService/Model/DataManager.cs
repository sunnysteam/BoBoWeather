using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using LifeService.Model.ReminderModel;
using System.Threading.Tasks;
using LifeService.Model.WeatherModel;

namespace LifeService.Model
{
    /// <summary>
    /// 数据管理类
    /// </summary>
    internal class DataManager
    {
        private static DataManager _instance = null;
        private static readonly object Padlock = new object();
        public SqlLite SqlLite;
        public List<ContactsModel> Contacts = new List<ContactsModel>();
        private DataManager()
        {
        }

        public static DataManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataManager();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 读取初始数据
        /// </summary>
        private void SetOriginalData()
        {
            ContactsModel contacts = new ContactsModel();
            DataTable datas = SelectTables(contacts);
            foreach (DataRow dataRow in datas.Rows)
            {
                ContactsModel contact = new ContactsModel();
                contact.Name = dataRow["Name"].ToString();
                contact.PhoneNumber = dataRow["PhoneNumber"].ToString();
                Contacts.Add(contact);
            }
        }

        /// <summary>
        /// 开始连接数据库
        /// </summary>
        public void StartConnect()
        {
            string path= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\LifeServiceData.db");
            //string path = "Database\\LifeServiceData.db";
            SqlLite = new SqlLite(path);
            if (!File.Exists(path))//检验是否有该路径，没有则新建
            {
                SQLiteConnection.CreateFile(path);
                CreateTables();
            }
            SetOriginalData();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnect()
        {
            SqlLite.Dispose();
        }

        /// <summary>
        ///在指定数据库中创建一个table
        /// </summary>
        private void CreateTables()
        {
            SqlLite.ExecuteNonQuery(ReminderModel.ReminderModel.CreateSql);
            SqlLite.ExecuteNonQuery(AlarmClockModel.AlarmClockModel.CreateSql);
            SqlLite.ExecuteNonQuery(ContactsModel.CreateSql);
            SqlLite.ExecuteNonQuery(WeatherModel.WeatherModel.CreateCitySql);
        }

        public void InsertTables(object data)
        {
            Type t = data.GetType();
            switch (t.Name)
            {
                case "ReminderModel":
                    Insert((data as ReminderModel.ReminderModel).InsertSql());
                    break;
                case "AlarmClockModel":
                    Insert((data as AlarmClockModel.AlarmClockModel).InsertSql());
                    break;
                case "WeatherModel":
                    Insert((data as WeatherModel.WeatherModel).InsertSql());
                    break;
            }
        }

        public void UpdateTables(object data)
        {
            Type t = data.GetType();
            switch (t.Name)
            {
                case "WeatherModel":
                    Update((data as WeatherModel.WeatherModel).UpdateSql());
                    break;
                case "AlarmClockModel":
                    Update((data as AlarmClockModel.AlarmClockModel).UpdateSql());
                    break;
                case "ReminderModel":
                    Update((data as ReminderModel.ReminderModel).UpdateSql());
                    break;
            }
        }

        public void DeleteTables(object data)
        {
            Type t = data.GetType();
            switch (t.Name)
            {
                case "AlarmClockModel":
                    Delete((data as AlarmClockModel.AlarmClockModel).DeleteSql());
                    break;
                case "ReminderModel":
                    Delete((data as ReminderModel.ReminderModel).DeleteSql());
                    break;
            }
        }

        public DataTable SelectTables(object model)
        {
            try
            {
                DataTable dataTable = new DataTable();
                Type t = model.GetType();
                switch (t.Name)
                {
                    case "ReminderModel":
                        dataTable = Select((model as ReminderModel.ReminderModel).SelectSql());
                        break;
                    case "ContactsModel":
                        dataTable = Select((model as ContactsModel).SelectSql());
                        break;
                    case "AlarmClockModel":
                        dataTable = Select((model as AlarmClockModel.AlarmClockModel).SelectSql());
                        break;
                    case "WeatherModel":
                        dataTable = Select((model as WeatherModel.WeatherModel).SelectSql());
                        break;
                    case "CityModel":
                        dataTable = Select((model as CityModel).SelectSql());
                        break;
                }
                return dataTable;
            }
            catch (Exception e)
            {
                LogWriter.Instance.Error(e);
                return null;
            }
        }

        private void Insert(string sql)
        {
            SqlLite?.ExecuteNonQuery(sql);
        }

        private void Update(string sql)
        {
            SqlLite?.ExecuteNonQuery(sql);
        }

        private void Delete(string sql)
        {
            SqlLite?.ExecuteNonQuery(sql);
        }

        private DataTable Select(string sql)
        {
            return SqlLite?.ExecuteQuery(sql);
        }
    }
}
