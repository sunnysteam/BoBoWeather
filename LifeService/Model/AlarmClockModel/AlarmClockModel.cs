using System;
using System.Data;

namespace LifeService.Model.AlarmClockModel
{
    /// <summary>
    /// 闹钟模型
    /// </summary>
    internal class AlarmClockModel:IDataModel
    {
        //创建
        public static string CreateSql = @"CREATE TABLE [alarmclock](
                                                [guid] NVARCHAR2(32) NOT NULL UNIQUE, 
                                                [alarmtime] NVARCHAR2(20) NOT NULL, 
                                                [alarmdate] NVARCHAR2(60) NOT NULL, 
                                                [label] NVARCHAR2(16), 
                                                [ring] NVARCHAR2(20), 
                                                [ringpath] NVARCHAR2(100), 
                                                [isrepetition] NVARCHAR2(1), 
                                                [IsOpen] NVARCHAR2(1), 
                                                PRIMARY KEY([guid]));";

        /// <summary>
        /// 唯一值
        /// </summary>
        public string GuiD { get; set; }

        /// <summary>
        /// 闹钟时间
        /// </summary>
        public string AlarmTime { get; set; }

        /// <summary>
        /// 闹钟日期
        /// </summary>
        public string AlarmDate { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 铃声
        /// </summary>
        public string Ring { get; set; }

        /// <summary>
        /// 铃声路径
        /// </summary>
        public string RingPath { get; set; }

        /// <summary>
        /// 是否稍后提醒
        /// </summary>
        public string IsRepetition { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public string IsOpen { get; set; }

        public string InsertSql()
        {
            return String.Format("insert into alarmclock values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", GuiD, AlarmTime, AlarmDate, Label, Ring, RingPath, IsRepetition, IsOpen);
        }

        public string UpdateSql()
        {
            return String.Format("update alarmclock set alarmtime='{0}',alarmdate='{1}',label='{2}',ring='{3}',ringpath='{4}',isrepetition='{5}',isopen='{6}' where guid='{7}'", AlarmTime, AlarmDate, Label, Ring, RingPath, IsRepetition, IsOpen, GuiD);
        }

        public string DeleteSql()
        {
            return String.Format("delete from alarmclock where guid='{0}'", GuiD);
        }

        public string SelectSql()
        {
            return "select * from alarmclock";
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="data"></param>
        public void FetchData(DataRow data)
        {
            GuiD = data["GuiD"].ToString();
            AlarmTime = data["AlarmTime"].ToString();
            AlarmDate = data["AlarmDate"].ToString();
            Label = data["Label"].ToString();
            Ring = data["Ring"].ToString();
            RingPath = data["RingPath"].ToString();
            IsRepetition = data["IsRepetition"].ToString();
            IsOpen = data["IsOpen"].ToString();
        }
    }
}
