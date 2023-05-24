using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace WebDemo
{

    public static class DbHelper
    {

        private static string _connectionStr;

        //静态构造方法 读取到配置文件
        //只会在第一次访问该类的时候执行一次静态构造
        static DbHelper()
        {
            ConfigurationBuilder configuration = new();
            //读取配置文件
            IConfigurationRoot config = configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(file =>
            {
                file.Path = "/appsettings.json";
                file.Optional = false;
                file.ReloadOnChange = true;
            }).Build();
            _connectionStr = config.GetConnectionString("SqlStudent");
        }

        /// <summary>
        /// 私有化一个连接数据库方法
        /// </summary>
        /// <returns>一个实例化的连接对象</returns>
        private static SqlConnection ConnectionAndOpen()
        {
            SqlConnection conn = new SqlConnection(_connectionStr);

            // 确保打开连接
            if (conn.State == ConnectionState.Broken)
            {
                conn.Close();
                conn.Open();
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }

        /// <summary>
        /// 私有化一个操作数据库命令的方法
        /// </summary>
        /// <param name="tSql">待执行的 T-SQL 语句</param>
        /// <param name="parameters">需要用到的参数</param>
        /// <returns>执行的命令对象</returns>
        private static SqlCommand OperateCommand(string tSql, params SqlParameter[]? parameters)
        {
            // 实例化 命令对象
            SqlCommand cmd = new SqlCommand(tSql, ConnectionAndOpen());

            // 对参数的判空操作，当没有参数时不用传递参数
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            return cmd;
        }

        /// <summary>
        /// 通常用于执行 增、删、改 操作
        /// </summary>
        /// <param name="tSql">待执行的 T-SQL 命令</param>
        /// <param name="parameters">需要传入的SQL参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string tSql, params SqlParameter[]? parameters)
        {
            SqlConnection conn = ConnectionAndOpen();

            try
            {

                using SqlCommand command = OperateCommand(tSql, parameters);
                
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);   // 打印错误
                return -1;  //  返回非正常值
            }
            finally // 无论查询结果如何，都关闭连接，减少资源消耗
            { 
                conn.Close();   //
                if (DbHelper.ConnectionAndOpen().State == ConnectionState.Closed)
                {
                    Debug.WriteLine("数据库连接已关闭");
                }
            }

        }

        /// <summary>
        /// 获取 T-SQL 语句执行后的首行首列的信息
        /// </summary>
        /// <param name="tSql">待执行的 T-SQL 命令</param>
        /// <param name="parameters">需要传入的SQL参数</param>
        /// <returns>首行首列的数据</returns>
        public static int ExecuteScalar(string tSql, params SqlParameter[] parameters)
        {
            SqlConnection conn = ConnectionAndOpen();

            try
            {

                using SqlCommand command = OperateCommand(tSql, parameters);

                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);   // 打印错误
                return -1;  //  返回非正常值
            }
            finally // 无论查询结果如何，都关闭连接，减少资源消耗
            {
                conn.Close();   //
                if (DbHelper.ConnectionAndOpen().State == ConnectionState.Closed)
                    Debug.WriteLine("数据库连接已关闭");
            }
        }

        /// <summary>
        /// 通过适配器读取数据并存入到 DataTable
        /// </summary>
        /// <param name="tSql">待执行的 T-SQL 命令</param>
        /// <param name="parameters">需要传入的SQL参数</param>
        /// <returns>DataTbale</returns>
        public static DataTable? GetDataTable(string tSql, params SqlParameter[] parameters)
        {
            SqlConnection conn = ConnectionAndOpen();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(tSql, conn);

                // 对参数的判空操作，当没有参数时不用传递参数
                if (parameters != null && parameters.Length > 0)
                    adapter.SelectCommand.Parameters.AddRange(parameters);
             
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.Message);   // 打印错误
                return null;  //  返回 null
            }
            finally
            {
                conn.Close();   //关！！
                if (DbHelper.ConnectionAndOpen().State == ConnectionState.Closed)
                    Debug.WriteLine("数据库连接已关闭");
            }
            
        }
    }
}
