using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using Vison_Inspect_System._2_ComPart;

namespace Vison_Inspect_System
{
    class MYSQL
    {
        public string ConnectionString { get; set; }
        //1.创建连接字符串
        //public const string connstring = "Server=localhost;Port=3306;User Id=root;password=lxw201314wq;Database=lxw;Charset=utf8";
        /// <summary>
        /// 创建数据库连接字符串
        /// </summary>
        /// <param name="server">服务器</param>
        ///    /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="database">数据库</param>
        /// <param name="port">端口</param>
        public MYSQL(string server,
             string username,
            string password,
            string database,
            int port = 3306
           )
        {
            ConnectionString = "Server=" + server + //创建数据库连接串
                               ";Port=" + port +
                               ";User ID=" + username +
                               ";Password=" + password +
                               ";DataBase=" + database +
                               ";Charset=utf8";
        }
        /// <summary>
        /// 数据库增，删，改
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="prams">SQL参数</param>
        /// <returns>影响数据库的行数</returns>
        public int ExecSQL(string sqlString, //命令文本
            object[] prams) //参数对象
        {
            var nRet = 0;
            MySqlCommand cmd;
            MySqlTransaction transaction = null;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new Exception("ConnectionString Error(注：创建数据库连接字符串错误！)");
            }

            if (string.IsNullOrEmpty(sqlString))
            {
                throw new Exception("SQL String Error（注：SQL语句为空错误！）");
            }

            if (!DsafConnectionTest())
            {
                throw new Exception("Database Connect Error（注：数据库连接错误！）");
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = ConnectionString;
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    cmd = CreateCommand(sqlString, prams, conn); //创建SqlCommand命令对象
                    cmd.Transaction = transaction;
                    nRet = cmd.ExecuteNonQuery(); //执行SQL命令
                    transaction.Commit();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (MySqlException)
            {
                transaction?.Rollback();
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                // ignored
            }
            return nRet;
        }
        /// <summary>
        /// 数据库增，删，改(无SQL参数)
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <returns></returns>
        public int ExecSQL(string sqlString)
        {
            var nRet = 0;

            MySqlCommand cmd;
            MySqlTransaction transaction = null;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new Exception("ConnectionString Error");
            }

            if (string.IsNullOrEmpty(sqlString))
            {
                throw new Exception("SQL String Error");
            }

            if (!DsafConnectionTest())
            {
                throw new Exception("Database Connect Error");
            }

            try
            {
                using (var conn = new MySqlConnection())
                {
                    conn.ConnectionString = ConnectionString;
                    conn.Open();

                    transaction = conn.BeginTransaction();
                    cmd = new MySqlCommand(sqlString, conn); //创建SqlCommand命令对象
                    cmd.Transaction = transaction;
                    nRet = cmd.ExecuteNonQuery(); //执行SQL命令
                    transaction.Commit();

                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                //transaction?.Rollback();
                //throw new Exception(ex.Number.ToString());
            }

            catch (Exception)
            {
                transaction?.Rollback();
            }

            //返回>0，表示执行成功
            return nRet;
        }


        /// <summary>
        /// 数据库查询
        /// </summary>
        /// <param name="sqlString">SQL查询语句</param>
        /// <param name="prams">SQL参数</param>
        /// <returns></returns>
        public DataTable ExecSQLQuery(string sqlString, object[] prams)
        {
            MySqlDataAdapter Adapter;
            DataTable table = null;
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new Exception("ConnectionString Error(注：创建数据库连接字符串错误！)");
            }
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new Exception("SQL String Error（注：SQL语句为空错误！）");
            }
            if (!DsafConnectionTest())
            {
                throw new Exception("Database Connect Error（注：数据库连接错误！）");
            }
            try
            {
                using (var conn = new MySqlConnection())
                {
                    conn.ConnectionString = ConnectionString;
                    conn.Open();
                    Adapter = CreateDataAdaper(sqlString, prams, conn); //创建桥接器对象
                    table = new DataTable(); //创建表对象
                    Adapter.Fill(table); //填充数据表

                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
            return table; //返回数据表
        }
        /// <summary>
        /// 检查数据库连接情况
        /// </summary>
        /// <returns></returns>
        private bool DsafConnectionTest()
        {
            bool bRet = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = ConnectionString;
                    conn.Open();
                    bRet = true;
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
            return bRet;
        }
        /// <summary>
        ///     创建一个SqlCommand对象以此来执行命令文本
        /// </summary>
        private MySqlCommand CreateCommand(string procName, //命令文本
            object[] prams,
            MySqlConnection conn) //命令文本所需参数
        {
            var cmd = new MySqlCommand(procName, conn); //创建SqlCommand命令对象
            cmd.CommandType = CommandType.Text; //指定要执行的类型为命令文本

            // 依次把参数传入命令文本
            if (prams != null) //判断SQL参数是否不为空
            {
                foreach (var parameter in prams) //遍历传递的每个SQL参数
                {
                    var param = (MySqlParameter)parameter;
                    cmd.Parameters.Add(param); //将SQL参数添加到执行命令对象中
                }
            }

            //加入返回参数
            cmd.Parameters.Add(new MySqlParameter("ReturnValue",
                MySqlDbType.Int32,
                4,
                ParameterDirection.ReturnValue,
                false,
                0,
                0,
                string.Empty,
                DataRowVersion.Default,
                null));

            //返回SqlCommand命令对象
            return cmd;
        }

        /// <summary>
        ///     创建一个SqlDataAdapter对象以此来执行命令文本
        /// </summary>
        private MySqlDataAdapter CreateDataAdaper(string sqLstring, //命令文本
            object[] prams,
            MySqlConnection conn) //参数对象
        {
            var dap = new MySqlDataAdapter(sqLstring, conn); //创建桥接器对象
            dap.SelectCommand.CommandType = CommandType.Text; //指定要执行的类型为命令文本

            if (prams != null) //判断SQL参数是否不为空
            {
                foreach (var parameter in prams) //遍历传递的每个SQL参数
                {
                    var param = (MySqlParameter)parameter;
                    dap.SelectCommand.Parameters.Add(param); //将SQL参数添加到执行命令对象中
                }
            }

            //加入返回参数
            dap.SelectCommand.Parameters.Add(new MySqlParameter("ReturnValue",
                MySqlDbType.Int32,
                4,
                ParameterDirection.ReturnValue,
                false,
                0,
                0,
                string.Empty,
                DataRowVersion.Default,
                null));


            //返回桥接器对象
            return dap;
        }
    }

}
