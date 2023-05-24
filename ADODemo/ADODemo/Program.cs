using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace ADODemo
{
    internal class Program
    {
        static string connStr = ConfigurationManager.ConnectionStrings["localLogin"].ConnectionString;
        static string conStr = ConfigurationManager.ConnectionStrings["localStudent"].ConnectionString;

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            //Console.WriteLine(ConfigurationManager.AppSettings["webName"]);
            
            


            Console.WriteLine("按任意键结束！");
            Console.ReadKey();
        }

        /// <summary>
        /// SqlConnection 连接数据库 
        /// </summary>
        private static void SQLConnection()
        {
            // 创建连接 => 告知连接对象、服务器、用户名、密码、数据库
            #region 连接方式一
            SqlConnection sqlConn = new();
            sqlConn.ConnectionString = "server=localhost;uid=sa;pwd=0825;database=Student;timeout=30";
            //Console.WriteLine(sqlConn.State);
            //Console.WriteLine(sqlConn.Database);
            //Console.WriteLine(sqlConn.ConnectionTimeout);
            #endregion

            #region 连接方式二
            //SqlConnection sqlConnection = new("server=localhost;uid=sa;pwd=0825;database=Student");
            //sqlConnection.Open();
            //Console.WriteLine(sqlConnection.State);
            //Console.WriteLine(sqlConnection.Database);
            //Console.WriteLine(sqlConnection.ServerVersion);
            //Console.WriteLine(sqlConnection.ConnectionTimeout);
            //sqlConnection.Close();
            //Console.WriteLine(sqlConnection.State);
            #endregion
        }

        /// <summary>
        /// SqlCommand 数据库指令对象
        /// </summary>
        /// <param name="command">传入指令对象</param>
        /// <param name="connection">指令对象要关联的数据库</param>
        private static void SQLCommand(SqlCommand command, SqlConnection connection)
        {
            command.Connection = connection;    // 建立关联
            command.CommandText = "T-SQL";  // 这里写 T-SQL语句
            command.ExecuteNonQuery();  // 执行 T-SQL 语句，返回受影响的行数，一般用于 增、删、该
            //command.ExecuteScalar();    // 执行 T-SQL 语句，返回查到的数据表格中的第一行第一列的值。一般用于统计总数、平均数等
            //command.ExecuteReader();    // 执行 T-SQL 语句，返回一个 SqlDataReader 对象

            //command.Cancel();   // 尝试取消 T-SQL 语句的执行
            command.Dispose();  // 释放资源
        }

        /// <summary>
        /// SqlDataReader 连接模式操作数据库
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        private static void SQLDataReader(SqlCommand command, SqlConnection connection)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "Select * from StudentInfo";  //设置 T-SQL 语句
            SqlDataReader reader = command.ExecuteReader();

            #region 遍历出查到所有的记录
            for (int i = 0; reader.Read(); i++)
            {
                Console.Write(i + 1 + "\t");

                for (int j = 0; j < reader.FieldCount; j++)
                    Console.Write(reader.GetValue(j) + "\t");

                Console.WriteLine();
            }
            #endregion

            //Console.WriteLine(reader.HasRows);

            if (!reader.IsClosed)
            {
                reader.Close(); // 关闭 reader 实例
            }

            reader.Dispose();   // SqlDataReader 释放资源
            command.Dispose();  // SqlCommand 释放资源  
            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// SqlDataApdater 适配器
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        private static void SqlDataApdater(SqlConnection connection)
        {

            SqlDataAdapter apdater = new SqlDataAdapter();  //实例化适配器
            apdater.SelectCommand = new SqlCommand("Select * from StudentInfo", connection);    //实例化命令对象

            DataSet dataSet = new DataSet(); //实例化DataSet
            apdater.Fill(dataSet);  //将数据填充到DataSet

            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// DataTable 简单使用
        /// </summary>
        private static void DataTableTest()
        {
            SqlConnection connection = new("server=localhost;uid=sa;pwd=0825;database=Student");

            SqlDataAdapter apdater = new SqlDataAdapter();  //实例化适配器
            apdater.SelectCommand = new SqlCommand("Select * from StudentInfo", connection);    //实例化命令对象
            // apdater.DeleteCommand = new SqlCommand("delete from StudentInfo", connection);   //

            DataTable dataTable = new DataTable();
            apdater.Fill(dataTable);  //将数据填充到DataTable

            #region 将数据以对象类型存入List中
            List<Student> studentInfo = new List<Student>();
            foreach (DataRow item in dataTable.Rows)
            {
                Student student = new Student(item["stuId"].ToString(), item["stuName"].ToString(),
                    Convert.ToInt32(item["classId"]), item["stuPhone"].ToString(),
                    item["stuSex"].ToString(), item["stuBirthday"].ToString());
                studentInfo.Add(student);
            }
            #endregion

            #region 遍历List中的元素
            foreach (var item in studentInfo)
            {
                Console.WriteLine($"{item.stuId}\t{item.stuName}\t{item.classId}\t{item.stuPhone}\t{item.stuSex}\t{item.stuBirthday}");
            }
            #endregion

            Console.WriteLine(connection.State);

            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// 手动模拟 DataTable 的数据
        /// </summary>
        private static void DataTableData()
        {
            DataTable dataTable = new DataTable();

            #region 创建列名
            dataTable.Columns.Add(new DataColumn("id"));
            dataTable.Columns.Add(new DataColumn("name"));
            dataTable.Columns.Add(new DataColumn("sex"));
            dataTable.Columns.Add(new DataColumn("phone"));
            #endregion

            #region 创建行 与 行数据
            DataRow row = dataTable.NewRow();
            row["id"] = 1;
            row["name"] = "刘德华";
            row["sex"] = "女";
            row["phone"] = "12345678999";
            #endregion

            #region 将数据添加到DataTable
            dataTable.Rows.Add(row);
            #endregion
        }

        /// <summary>
        /// 读取配置文件连接数据库
        /// </summary>
        private static void AppConfiguration()
        {
            // 获取配置文件中的连接字符串
            string conStr = ConfigurationManager.ConnectionStrings["localStudent"].ConnectionString;

            SqlConnection conn = new SqlConnection(conStr); // 实例化连接对象
            Console.WriteLine(conn.Database);


            conn.Close();
            conn.Dispose();
        }

        /// <summary>
        /// 修改配置文件中的 pooling 的值，进行性能比较
        /// </summary>
        private static void ComparePerformance()
        {
            string conStr = ConfigurationManager.ConnectionStrings["localStudent"].ConnectionString;
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                SqlConnection connection = new(conStr);
                connection.Open();

                connection.Close();
                connection.Dispose();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 1000.0 + "s");
        }

        /// <summary>
        /// SqlParameter 无参构造方法的案例
        /// </summary>
        private static void ParameterSelect()
        {
            //string connStr = ConfigurationManager.ConnectionStrings["localLogin"].ConnectionString;
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();

            string sql = "Select COUNT(*) from LoginData where Account=@account and Pwd=@pwd";
            SqlCommand command = new SqlCommand(sql, connection);

            SqlParameter parameter1 = new SqlParameter();
            parameter1.ParameterName = "account";   //设置参数名
            parameter1.SqlDbType = SqlDbType.VarChar;   // 设置类型
            parameter1.Size = 20;    //参数大小
            parameter1.Value = "刘德华";   //为参数赋值

            SqlParameter parameter2 = new SqlParameter();
            parameter2.ParameterName = "pwd";   //设置参数名
            parameter2.SqlDbType = SqlDbType.VarChar;   // 设置类型
            parameter2.Size = 20;    //参数大小
            parameter2.Value = "123456";   //为参数赋值

            command.Parameters.Add(parameter1);
            command.Parameters.Add(parameter2);

            Object? obj = command.ExecuteScalar();
            Console.WriteLine(Convert.ToInt32(obj));
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }
    
        /// <summary>
        /// 通过参数添加数据
        /// </summary>
        private static void ParameterInsert()
        {
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();

            string sql = "Insert into LoginData values(@account, @pwd)";
            SqlCommand command = new SqlCommand(sql, connection);

            SqlParameter[] parameters = //创建参数对象
            {
                new SqlParameter("account", "zhangxueyou"),
                new SqlParameter("pwd", "000000")
            };

            command.Parameters.AddRange(parameters);    //批量添加参数对象，传入参数数组

            Object obj = command.ExecuteNonQuery();
            if (Convert.ToInt32(obj) > 0)
            {
                Console.WriteLine("添加成功");
            }
            else
            {
                Console.WriteLine("添加失败");
            }
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }
    
        /// <summary>
        /// 通过参数更新数据
        /// </summary>
        private static void ParameterUpdate()
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                string sql = "Update LoginData set Account=@account, Pwd=@pwd where Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // 创建参数数组对象
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("account", SqlDbType.VarChar, 20),
                        new SqlParameter("pwd", SqlDbType.VarChar, 20),
                        new SqlParameter("id", SqlDbType.Int, 1)
                    };

                    // 
                    parameters[0].Value = "Lucky";
                    parameters[1].Value = "0000";
                    parameters[2].Value = 1;

                    command.Parameters.AddRange(parameters);    //批量添加参数对象，传入参数数组 

                    Object obj = command.ExecuteNonQuery();

                    if (Convert.ToInt32(obj) > 0)   Console.WriteLine("修改成功");
                    else    Console.WriteLine("修改失败");
                }
            }
        }

        /// <summary>
        /// 通过参数查询数据
        /// </summary>
        private static void ParameterSelectData()
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string sql = "Select * from StudentInfo where stuName like @stuName and stuSex=@sex";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                // 参数
                SqlParameter[] parameters =
                {
                    new SqlParameter("stuName", SqlDbType.VarChar, 20),
                    new SqlParameter("sex", SqlDbType.Char, 2)
                };
                parameters[0].Value = "%张%";
                parameters[1].Value = '女';

                adapter.SelectCommand.Parameters.AddRange(parameters);

                DataTable table = new();    // 实例化一个数据表
                adapter.Fill(table);    // 填充数据


            }
        }

        /// <summary>
        /// 通过参数删除数据
        /// </summary>
        private static void ParameterDelete()
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                string sql = "Delete from StudentInfo where stuId=@stuId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("stuId", 180325023));

                    Object obj = cmd.ExecuteNonQuery();
                    Console.WriteLine(Convert.ToInt32(obj));
                }
            }
        }

        /// <summary>
        /// 结合事务操作
        /// </summary>
        private static void ParameterTransaction()
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();    // 打开连接
                SqlTransaction transaction = conn.BeginTransaction();   // 开启事务

                try
                {
                    string sql = "Update StudentInfo set classId=@classId where stuId=@stuId;"
                        + "Update StudentScore set courseName=@courseName, theoryScore=@theoryScore, skillScore=@skillScore"
                        + " where stuId=@stuId;";    // T-SQL 
                    using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
                    {
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("classId", SqlDbType.Int),
                            new SqlParameter("stuId", SqlDbType.Char, 10),
                            new SqlParameter("courseName", SqlDbType.VarChar, 20),
                            new SqlParameter("theoryScore", SqlDbType.Int),
                            new SqlParameter("skillScore", SqlDbType.Int)
                        };

                        parameters[0].Value = 3;
                        parameters[1].Value = "180325011";
                        parameters[2].Value = "会计2班";
                        parameters[3].Value = 100;
                        parameters[4].Value = 98;

                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();

                        transaction.Commit();   //提交事务
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback(); //发生异常时回滚，保证数据的完整性
                    throw;
                }
                finally
                {
                    transaction.Dispose();  //释放资源
                }
            }
        }

        /// <summary>
        /// 无输出参数的存储过程
        /// </summary>
        private static void ProcedureNonOP()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("up_logindata_insert", conn))
                {
                    //默认是CommandType.Text, 若不修改则视字符串为T-SQL语句
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@account", SqlDbType.VarChar, 20),
                        new SqlParameter("@pwd", SqlDbType.VarChar, 20)
                    };
                    parameters[0].Value = "lucky dog";
                    parameters[1].Value = "lucky";
                    cmd.Parameters.AddRange(parameters);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        /// <summary>
        /// 有输出参数的存储过程
        /// </summary>
        private static void ProcedureOP()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new("up_logindata_insert2", conn);
                cmd.CommandType = CommandType.StoredProcedure;  //修改默认值

                SqlParameter[] parameters =
                {
                    new SqlParameter("account", SqlDbType.VarChar, 20),
                    new SqlParameter("pwd", SqlDbType.VarChar, 20),
                    new SqlParameter("code", SqlDbType.Int),    // 输出参数
                    new SqlParameter("msg", SqlDbType.VarChar, 20)  //输出参数
                };
                parameters[0].Value = "human";
                parameters[1].Value = "0000";
                parameters[2].Direction = ParameterDirection.Output;    // 修改参数的方向为 output
                parameters[3].Direction = ParameterDirection.Output;    // 默认方向是 Input

                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();

                // 执行存储过程后才能拿到输出参数
                string? msg = parameters[3].Value?.ToString();
                int code = Convert.ToInt32(parameters[2].Value);
                Console.WriteLine($"code:{code}\t msg:{msg}");
            } 
        }

    }

    record Student(string stuId, string stuName, int classId, string stuPhone, string stuSex, string stuBirthday);
}