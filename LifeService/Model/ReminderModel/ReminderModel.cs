using System;
using System.Data;

namespace LifeService.Model.ReminderModel
{
    /// <summary>
    /// 提醒模型
    /// </summary>
    internal class ReminderModel : IDataModel
    {
        //创建
        public static string CreateSql = @"CREATE TABLE [reminder](
                                                [guid] varchar2(32) PRIMARY KEY NOT NULL, 
                                                [remindertime] NVARCHAR2(20) NOT NULL, 
                                                [content] NVARCHAR2(140), 
                                                [contact] NVARCHAR2(50), 
                                                [record] NVARCHAR2(40), 
                                                [recordpath] NVARCHAR2(200),
                                                [ring] NVARCHAR2(100));";

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string GuiD { get; set; }

        /// <summary>
        /// 提醒时间
        /// </summary>
        public string ReminderTime { get; set; }

        /// <summary>
        /// 提醒内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 语音
        /// </summary>
        public string Record { get; set; }

        /// <summary>
        /// 语音路径
        /// </summary>
        public string RecordPath { get; set; }

        /// <summary>
        /// 铃声
        /// </summary>
        public string Ring { get; set; }

        public string InsertSql()
        {
            return String.Format("insert into reminder values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", GuiD, ReminderTime,
                Content, Contact, Record, RecordPath, Ring);
        }

        public string UpdateSql()
        {
            return
                String.Format(
                    "update reminder set ReminderTime='{0}',Content='{1}',Contact='{2}',Record='{3}',RecordPath='{4}',Ring='{5}' where guid='{6}'",
                    ReminderTime, Content, Contact, Record, RecordPath, Ring, GuiD);
        }

        public string DeleteSql()
        {
            return String.Format("delete from reminder where guid='{0}'", GuiD);

        }

        public string SelectSql()
        {
            return "select * from reminder";
        }

        public void FetchData(DataRow data)
        {
            GuiD = data["GuiD"].ToString();
            ReminderTime = data["ReminderTime"].ToString();
            Content = data["Content"].ToString();
            Contact = data["Contact"].ToString();
            Record = data["Record"].ToString();
            RecordPath = data["RecordPath"].ToString();
            Ring = data["Ring"].ToString();
        }
    }
}
