
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace LifeService.Model
{
    /// <summary>
    /// 数据库接口类
    /// </summary>
    internal class SqlLite : IDisposable
    {
        private string _datasource = @"Database\Data.db";
        private bool _isOpen;
        private bool _disposed = false;
        private SQLiteConnection _connection;
        private Dictionary<string, string> _parameters;


        public SqlLite()
        {
            Init("");
        }

        /// <summary>
        /// 连接指定的数据库
        /// </summary>
        /// <param name="datasource">连接字符串</param>
        public SqlLite(string datasource)
        {
            Init(datasource);
        }

        /**/

        /// <summary>
        /// 清理托管资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /**/

        /// <summary>
        /// 清理所有使用资源
        /// </summary>
        /// <param name="disposing">如果为true则清理托管资源</param>
        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                // dispose all managed resources.
                this?.Close();
                if (disposing)
                {
                    this._isOpen = false;
                    _connection?.Dispose();
                }
                // dispose all unmanaged resources
                
                _disposed = true;
            }
        }

        ~SqlLite()
        {
            Dispose(false);
        }


        #region Function

        private void Init(string datasource)
        {
            try
            {
                if (datasource != "")
                    this._datasource = datasource;
                this._connection = new SQLiteConnection("data source = " + this._datasource);
                this._parameters = new Dictionary<string, string>();
                this.Open();
                this._isOpen = true;
            }
            catch (Exception ex)
            {
                Init(datasource);
            }
        }

        private bool CheckDbExist()
        {
            if (System.IO.File.Exists(_datasource))
                return true;
            else
                return false;
        }

        private void Open()
        {
            try
            {
                if (!CheckDbExist())
                {
                }
                if (!_isOpen)
                    _connection.Open();
                this._isOpen = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Close()
        {
            //if (_isOpen && _connection != null)
            //    _connection?.Close();
            this._isOpen = false;
        }

        #endregion //Function

        #region Method

        /**/

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string key, string value)
        {
            _parameters.Add(key, value);
        }

        /**/

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        public void ExecuteNonQuery(string queryStr)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    Open();
                }
                using (SQLiteTransaction transaction = _connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(_connection))
                    {
                        command.CommandText = queryStr;
                        foreach (KeyValuePair<string, string> kvp in this._parameters)
                        {
                            command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                        }
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                this.Close();
                this._parameters.Clear();
            }
        }

        /**/

        /// <summary>
        /// 执行SQL语句并返回所有结果
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        /// <returns>返回DataTable</returns>
        public DataTable ExecuteQuery(string queryStr)
        {
            DataTable dt = new DataTable();
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    Open();
                }
                using (SQLiteCommand command = new SQLiteCommand(_connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    command.CommandText = queryStr;
                    foreach (KeyValuePair<string, string> kvp in this._parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                    }
                    adapter.Fill(dt);
                }
            }
            catch (Exception e)
            {
                this.Close();
                this._parameters.Clear();
            }
            return dt;
        }

        /**/

        /// <summary>
        /// 执行SQL语句并返回第一行
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        /// <returns>返回DataRow</returns>
        public DataRow ExecuteRow(string queryStr)
        {
            DataRow row = null;
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    Open();
                }
                using (SQLiteCommand command = new SQLiteCommand(_connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    command.CommandText = queryStr;
                    foreach (KeyValuePair<string, string> kvp in this._parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                    }
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 0)
                        row = null;
                    else
                        row = dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
                this.Close();
                this._parameters.Clear();
            }
            return row;
        }

        /**/

        /// <summary>
        /// 执行SQL语句并返回结果第一行的第一列
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        /// <returns>返回值</returns>
        public Object ExecuteScalar(string queryStr)
        {
            Object obj = new object();
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(_connection))
                {
                    command.CommandText = queryStr;
                    foreach (KeyValuePair<string, string> kvp in this._parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                    }
                    obj = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                this.Close();
                this._parameters.Clear();
            }
            return obj;
        }

        #endregion //Method
    }
}

