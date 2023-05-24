# ADO.NET 

## 1. Introduce

ADO(ActiveX Data Objects), 是一个 `COM` 组件库，用于 `Microsoft` 技术中访问数据。使用 `ADO.NET` 作为名称，是因为 `Microsoft` 希望在 `.NET` 编程环境中优先使用数据访问接口

`ADO.NET` 让开发人员以一致的方式存取资料来源（如 `SQL Server` 与 `XML` ），以及透过 `OLE DB` 和 `ODBC` 所公开的资料来源。

`ADO.NET` 是一组向 .NET Framework 程序员公开数据访问服务的类。 `ADO.NET` 为创建分布式数据共享应用程序提供了一组丰富的组件。

`ADO.NET` 可将资料管理的资料存取分成不连续的元件，这些元件可分开使用，也可串联使用 `ADO.NET` 也包含 `.NET Framework` 资料提供者，以用于连接资料库、执行命令和获取结果。这些结果会直接处理、放入 `ADO.NET DataSer` 物件中以便利用机器操作 (Ad Hoc) 的方式公开给使用者、与多 个来源的资料结合，或在各层之间进行传递。`Dataset` 物件也可以与 `.NET Framework` 资料提供者分开 使用，以便管理应用程序本机的资料或来自 `XML` 的资料。

`ADO.NET` 类 (Class) 位于 `System.Data.dll`中，而且会与 `System.Xml.dll` 中的 `XML` 类虽整合。

`ADO.NET` 可为撰写 `Managed` 程式码的开发人员提供类似于 `ActiveX DSS Objects` (ADO) 提供给 原生元件物件模型 (Component Object Model，COM) 开发信员的功能，建议使用 `ADO.NET` 而非 `ADO` 来存取 `.NET` 应用程序中的资料。

`ADO.NET` 会提供最直接的方法，让开发人员在 `.NET Framework` 中进行资料存取。

**优点：**

+ 执行效率高。任何 `ORM` 框架（对象关系映射，理解为一种可提高访问数据库开发效率的一种框架) ，`ado.net` 作为原装的直接跟数据库打交道，直接操作数据库，没有进行额外的封装。如可以直接执行 sql` 语句，直接调用存情过程。直接操作 `DataSet` 数据集等等数据

**缺点：**

+ 开发效率偏慢
+ 缺乏面向对象思想

## 2. Connection

`Connection` 是数据库连接对象，用于与数据库建立连援。`Sql Server` 连接类是 `SqlConnection`, `MySql` 的连接
类是 `MysqlConnection`, `Oracle` 数据库连接类是 `OracleConnection `，这些类都实现了 `IDbConnection` 接口。

### 2.1 SqlConnection

+ 命名空间：`System.Data.SqlClient`
+ 程序集：`System.Data.SqlClient.dll`

需要通过 `NuGet` 将 `System.Data.SqlClient` 程序集引用到项目

> **构造方法**
>
> `SqlConnection` 无参构造方法，通过属性 `ConnectionString` 指定连接服务器、用户名、密码以及数据库等
>
> `SqlConnection(string)` 给定包含连接字符串的字符串，则初始化 `SqlConnection` 类的新实例
>
> `SqlConnection(string, SqlCredential)` 在给定连接字符串的情况下。初始化 `SqlConnection` 类的新实例，该连接字符串不使用 `Intergrated Security = true` 和包含 用户ID 和 用户密码 的 `SqlCredential` 对象
>
> ---
>
> **属性**
>
> `ConnectionString` 连接字符串
>
> `State` 只读，连接状态
>
> `Database` 只读，当前连接的数据库
>
> `ServerVersion` 只读，当前客户端连接的 `SQL Server` 的版本
>
> `ConnectionTimeout` 尝试建立连接时终止尝试并生成错误之前所等待的时间（默认15秒）
>
> ---
>
> **常用方法**
>
> `Open()` 使用由 `ConnectionString` 指定的属性设置打开数据库连接
>
> `Close()` 关闭与数据库之间的连接，此方法是关闭任何打开连接的首选方法
>
> `Dispose()` 执行与释放或重置非托管资源关联的应用程序定义的任务
>
> `BeginTransaction()` 开始数据事务
>
> `BeginTransaction(lsolationLevel)` 指定的隔离级别启动数据库事务
>
> `BeginTransaction(lsolationLevel, string)` 指定的隔离级别和事务名称启动数据库事务
>
> `CreateCommand()` 创建并返回与 `SqlConnection` 关联的 `SqlCommand()` 对象
>
> ---

```csharp
using System.Data.SqlClient;
// 创建连接 => 告知连接对象、服务器、用户名、密码、数据库
SqlConnection sqlConn = new();	// 实例化连接对象
sqlConn.ConnectionString = "server=localhost;uid=sa;pwd=****;database=Student;timeout=30";	//连接字符串
sqlConn.Open();	// 打开连接
Console.WriteLine(sqlConn.State);
Console.WriteLine(sqlConn.ConnectionTimeout);
sqlConn.Close(); // 关闭连接

Console.WriteLine("--------------------------------------");

using(SqlConnection sqlConnection = new("server=localhost;uid=sa;pwd=****;database=Student;"))
{
    // ……
}
```

☆ 使用 `Open()` 要释放资源(`Close()`)。使用 `Using` 能够自动释放资源

##  3. Command

`Command` 是数据库命令对象，它是数据库执行的一个 `Transact-SQL` 语句或存储过程。`Sql Server` 连接类是 `SqlCommand`, `MySql `的连接类是`MysqlCommand`, `Oracle` 数据库连接类是 `OracleCommand` ，这些类都实现了 `IDbCommand` 接口。

### 3.1 SqlCommand

+ 命名空间 `System.Data.SqlClient`
+ 程序集 `System.Data.SqlClient.dll`

> **构造方法**
>
> `SqlCommand()` 无参构造方法，实例化一个 `SqlCommand` 对象
>
> `SqlCommand(string)` 使用查询文本初始化 `SqlCommand` 对象
>
> `SqlCommand(string, SqlConnection)` 使用 `查询文本` 与 连接对象(`SqlConnection`) 初始化 `SqlCommand` 对象
>
> `SqlCommand(string, SqlConnection, SqlTransaction)` 查询文本、连接对象和事务初始化 `SqlCommand` 对象
>
> ---
>
> **常用属性**
>
> `CommandText` 获取或设置要在数据库中执行的 `T-SQL` 语句、表名或存储过程
>
> `CommandTimeout` 获取或设置在终止尝试执行命令并生成错误之前的等待时间
>
> `CommandType` 获取或设置一个值，该值指示解释 `CommandText` 属性的方式
>
> `Connection` 获取 `SqlCommand` 的实例使用的 `SqlConnection` 
>
> `Parameters` 获取 `SqlParametersCollection`
>
> `Transaction` 获取或设置要在其中执行 `SqlTransaction` 的 `SqlCommand`
>
> ---
>
> **常用方法**
>
> `Cancel()` 尝试取消 `SqlCommand` 的执行。
>
> `CreateParameter()` 创建 `SqlParameter` 对象的新实例。
>
> `Dispose`() 执行与释放或重置非托管资源关联的应用程序定义的任务(继承自 `DbCommand` )
>
> `Dispose(Boolean)` 释放由 `SqlCommand` 占用的非托管资源，还可以另外再释放托管资源(继承自 `DbCommand` )
>
> `ExecuteNonQuery()` 对连接执行 `Transact-SQL` 语并返回受影响的行数
>
> `ExecuteScalar()` 执行查询，并返回查询所返回的结果集中第一行的第一列，忽略其他列和行。一般用于获取总条数
>
> `ExecuteReader()` 将 `CommandText` 发送到 `Connection`，并生成 `SqlDataReader` 
>
> ---

```csharp
/**
 * 1. 实例化 SqlConnection 对象
 * 2. 打开连接
 * 3. 实例化 SqlCommand 对象
 * 4. 指定连接对象
 * 5. 设置要执行的SQL语句
 * 6. 执行命令
 */
using(SqlConnection sqlConnection = new("server=localhost;uid=sa;pwd=****;database=Student;"))
{
    sqlConnection.Open();	// 打开连接
	using(SqlCommand sqlCommand = new())
    // using(SqlCommand command = new(指定执行的SQL语句, 连接对象))
	{
		sqlCommand.Connection = SqlConnection;	// 设置连接对象
        sqlCommand.CommandText = "Insert into tableName values(value1……)";	// 设置SQL语句
        SqlCommand.ExecuteNonQuery();	// 执行SQL
	}
}
```

```csharp
// 获取数据表中首行首列的信息
using(SqlConnection conn = new("server=localhost;uid=sa;pwd=****;database=Student;"))
{
    conn.Open();
    using(SqlCommand comm = new())
    {
        sqlCommand.Connection = SqlConnection;	// 设置连接对象
        comm.Connection = conn;
        comm.CommandText = "Select count(id) from StudentInfo";
        comm.ExecuteScalar();	// 结果只能得到 一个
    }
}
```

## 4. SqlDataReader

提供从 `SQL Server` 数据库中读取只进的行流的方式，在读取数据的过程中需要一直与数据库保持连接，适合数据量小的情况，执行效率还行。`SqlDataReader` 读取数据的方式称为<u>连接模式</u>。

+ 命名空间 `System.Data.SqlClient`
+ 程序集 `System.Data.SqlClient.dll` 

> **属性**
>
> `Connection`  获取 `SqlConnection` 关联的 `SqlDataReader`
>
> `Depth` 获取当前行的嵌套深度
>
> `FieldCount` 获取当前行中的列数
>
> `HasRows` 判断 `SqlDataReader` 是否包含一行或多行，得到`bool`值。如果是最后一行则返回`false`
>
> `IsClosed` 判断 `SqlDataReader` 实例是否关闭，得到 `bool` 值
>
> `Item[Int32]` 给定的序列号的情况下，获取指定列的本机格式表示的值
>
> `Item[String]` 给定列名称的情况下，获取指定列的本机格式表示的值
>
> `RecordsAffected` 获取执行 `T-SQL` 语句后受影响行数
>
> `VisibleFieldCount` 获取`SqlDataReader` 中未隐藏的字段的数目
>
> ---
>
> **常用方法**
>
> `Close()` 关闭`SqlDataReader`的实例
>
> `Dispose()` 释放 `SqlDataReader` 实例所使用的资源
>
> `GetNumber(Int32)` 获取指定列的名称
>
> `GetOrdinal(String)` 在给定列名时获取相应的序列号
>
> `Read()` 使`SqlDataReader` 读到下一行记录，返回值为`bool`类型
>
> ---

```csharp
SqlConnection connection = new SqlConnection("server=localhost;uid=sa;pwd=0825;database=Student");
connection.Open();	// 打开连接

SqlCommand command = connection.CreateCommand();	//创建指令对象
command.CommandText = "Select * from StudentInfo";	//设置要执行的 t-sql 语句
SqlDataReader reader = command.ExecuteReader();	//执行 t-sql ，返回SqlDataReader对象

// 遍历出从数据表中查到的记录
for (int i = 0; reader.Read(); i++)	//
{
	Console.Write(i + 1 + "\t");
    
	for (int j = 0; j < reader.FieldCount; j++)
		Console.Write(reader.GetValue(j) + "\t");
    
	Console.WriteLine();
}


connection.Close();
connection.Dispose();

// GetValue(IdIndex) : 获取当前行记录中指定列的数据
```

## 5. SqlDataAdapter

用于填充 `DataSet` (表示数据的内存中的数据库，可以由多个table组成) 和更新 `SQL Server` 数据库的一组数据命令和一个数据库连接。为 <u>适配器模式</u> 也称为 <u>断开模式</u>。

> 一次连接取得数据后即可断开，在用户非常多的情况下，可以减少连接池资源的占用
>
> 一次性的从数据库取得数据后，将数据存入内存中，不会再去操作数据库，对这些数据进行操作不会影响数据库中的内容

+ 命名空间 `System.Data.SqlClient`
+ 程序集 `System.Data.SqlClient.dll` 

> **构造方法**
>
> `SqlDataAdapter()` 初始化 `SqlDataAdapter()` 类的实例
>
> `SqlDataAdapter(SqlCommand)` 实例化对象，传入 `SqlCommand` 作为 `SelectCommand` 的属性
>
> `SqlDataAdapter(String, SqlConnection)` 使用`SqlDataAdapter`和`SelectCommand`对像初始化`SqlConnection`类的新实例
>
> `SqlDataAdpater(String, String)` 用`SqlDataAdapter`和一个连接字符串初始化`SelectCommand`类的一个新实例
>
> ---
>
> **属性**
>
> `DeleteCommand` 获取或设置一个 `T-SQL` 语句或存储过程，以从数据集删除记录
>
> `InsertCommand` 获取或设置一个 `T-SQL` 语句或存储过程，以在数据源中插入新记录
>
> `SelectCommand` 获取或设置一个 `T-SQL` 语句或存储过程，用于在数据源中选择记录
>
> `UpdateBathSize` 获取或设置每次到服务器的往返过程中处理的行数
>
> `UpdateCommand` 获取或设置一个 `T-SQL` 语句或存储过程，用于更新数据源中的记录
>
> ---

```csharp
// 实例化适配器
SqlConnection connection = new("server=localhost;uid=sa;pwd=0825;database=Student");
SqlDataAdapter apdater = new SqlDataAdapter();  //实例化适配器
apdater.SelectCommand = new SqlCommand("Select * from StudentInfo", connection);    //实例化命令对象
DataSet dataSet = new();    //实例化DataSet
apdater.Fill(dataSet);  //将数据填充到DataSet

connection.Close();
connection.Dispose();
```

```csharp
// DeleteCommand
SqlConnection connection = new("server=localhost;uid=sa;pwd=0825;database=Student");
SqlDataAdapter apdater = new SqlDataAdapter();  //实例化适配器
apdater.DeleteCommand = new SqlCommand("delete from StudentInfo", connection);

connection.Close();
connection.Dispose();
```

```csharp
// InsertCommand
SqlConnection connection = new("server=localhost;uid=sa;pwd=0825;database=Student");
SqlDataAdapter apdater = new SqlDataAdapter();  //实例化适配器
apdater.InsertCommand = new SqlCommand("Insert into StudentInfo values(XXX)", connection);

connection.Close();
connection.Dispose();
```



### 5.1 DataTable

内存中的数据表

通常将 `DataTbale` 封装成 `List` 集合。操作 `List`

- 命名空间 `System.Data`
- 程序集 `System.Data.Common.dll`

```csharp
// 准备一个类接收数据
record Student(string stuId, string stuName
               , int classId, string stuPhone
               , string stuSex, string stuBirthday);

SqlConnection connection = new("server=localhost;uid=sa;pwd=0825;database=Student");

SqlDataAdapter apdater = new SqlDataAdapter();  //实例化适配器
apdater.SelectCommand = new SqlCommand("Select * from StudentInfo", connection);    //实例化命令对象

DataTable dataTable = new DataTable();
apdater.Fill(dataTable);  //将数据填充到DataTable

List<Student> studentInfo = new List<Student>();	//

foreach (DataRow item in dataTable.Rows)
{
	Student student = new Student(item["stuId"].ToString(), item["stuName"].ToString(),
                                  Convert.ToInt32(item["classId"]), item["stuPhone"].ToString(),
                                  item["stuSex"].ToString(), item["stuBirthday"].ToString());
    studentInfo.Add(student);
}

foreach (var item in studentInfo)
{
	Console.WriteLine($"{item.stuId}\t"
                      + "{item.stuName}\t"
                      + "{item.classId}\t"
                      + "{item.stuPhone}\t
                      + "{item.stuSex}\t"
                      + "{item.stuBirthday}");
}

connection.Close();
connection.Dispose();
```

```csharp
// 模拟DataTable数据
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
```

## 6. 配置文件

服务器的变更、密码的修改等，导致连接字符串会发生变化。如果采用硬编码的形式写入代码中，会导致项目频繁部署，影响用户体验。通过配置文件的形式存储连接字符串，可以减少字符串的修改。

+ 文件名 `App.config`

+ 需要添加NuGet包 `System.Configuration.ConfigurationManager`

> `Install-package System.Configuration.ConfigurationManager`
>
> 1. 新建一个配置文件（`App.config`）
> 2. 在配置文件中写入连接服务器的字符串
> 3. 添加NuGet包，引用命名空间
> 4. 在程序中使用配置文件的连接字符串

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<connectionStrings>
		<!-- 本机连接字符串 -->
		<add name="localString" connectionString="server=localhost;uid=sa;pwd=0825;database=Student" />
	</connectionStrings>
</configuration>
```

```csharp
using System.Configuration;

string conStr = ConfigurationManager.ConnectionStrings["localString"].ConnectionString;
SqlConnection conn = new SqlConnection(conStr);
Console.WriteLine(conn.Database);
                        
conn.Close();
conn.Dispose();
```

### 6.1 AppSettings

配置文件不仅需要配置连接字符串，还可能需要配置其他东西时，则使用 `AppSettings` 节点进行配置

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<connectionStrings>
		
		<!-- 本机连接字符串 -->
		<add name="localString" connectionString="server=localhost;uid=sa;pwd=0825;database=Student" />
	</connectionStrings>
	
    <!-- 其他配置 -->
	<appSettings>
		<add key="webName" value="测试AppSetting"/>
	</appSettings>
</configuration>
```

```csharp
Console.WriteLine(ConfigurationManager.AppSettings["webName"]);
```

## 7. 数据连接池

数据库连接很耗费资源。通过连接池更有效地利用数据库连接的最重要措施，对于大型的应用系统的性能至关重要

`ADO.NETData Provider` 辅助管理连接池

`pooling=true` 

### 7.1 ADO.NET 连接池

是 `ADO.NETData Provider` 提供的一个机制，使得应用程序**使用的连接保存在连接池里**而避免每次都要完成 <u>建立/关闭</u> 连接的完整过程

> 收到连接请求时建立连接的完整过程是：
>
> + 在连接池建立新的连接（“逻辑连接”）
> + 建立该 “逻辑连接” 对应的 “物理连接” ☆
>
> ---
>
> 关闭一个连接的完整过程是：
>
> + 先关闭 “逻辑连接” 对应的 “物理连接” ☆
> + 销毁 “逻辑连接”

`SqlConnection.Open()` 是请求建立一个连接，不一定需要完成建立连接过程，可能只需要从连接池中取出一个可用的连接即可

`SqlConnection.Close()` 是请求关闭一个连接，不一定需要完成关闭连接的过程，可能只需要把连接释放回连接池即可

### 7.2 性能对比

开启连接池前

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<connectionStrings>
		
		<!-- 本机连接字符串 -->
		<add name="localString" connectionString="server=localhost;uid=sa;pwd=0825;database=Student;pooling=false" />
	</connectionStrings>
	
	<appSettings>
		<add key="webName" value="测试AppSetting"/>
	</appSettings>
</configuration>
```

```csharp
string conStr = ConfigurationManager.ConnectionStrings["localString"].ConnectionString;
Stopwatch sw = Stopwatch.StartNew();
for (int i = 0; i < 100000; i++)
{
	SqlConnection connection = new(conStr);
	connection.Open();
    
	connection.Close();
	connection.Dispose();
}
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds/1000.0 + "s");
```

---

开启连接池后

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<connectionStrings>
		
		<!-- 本机连接字符串 -->
		<add name="localString" connectionString="server=localhost;uid=sa;pwd=0825;database=Student;pooling=true" />
	</connectionStrings>
	
	<appSettings>
		<add key="webName" value="测试AppSetting"/>
	</appSettings>
</configuration>
```

```csharp
// 开启连接池后
string conStr = ConfigurationManager.ConnectionStrings["localString"].ConnectionString;
Stopwatch sw = Stopwatch.StartNew();
for (int i = 0; i < 100000; i++)
{
	SqlConnection connection = new(conStr);
	connection.Open();
    
	connection.Close();
	connection.Dispose();
}
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds/1000.0 + "s");
```

## 8. 参数化查询

将要传递的 `SQL` 参数不通过拼接的形式进行组装 `SQL` 语句，通过 `SQL 变量` 的形式进行传递

> 防止 SQL 注入攻击的手段
>
> + 存储过程
> + 参数化

### 8.1 SqlParameter

表示 `SqlCommand` 的参数，参数名称不区分大小写

> **构造方法**
>
> `SqlParameter()` 实例化`SqlParameter`对象
>
> `SqlParameter(string, object)` 实例化一个对象， 使用参数名称和新`SqlParameter`的值
>
> `SqlParameter(string, SqlDbType)` 使用参数名称和数据类型实例化一个对象
>
> ---
>
> **属性**
>
> `CompareInfo` 获取或设置`CinoareInfo`对象，定义如何对此参数执行字符串比较
>
> `DbType` 获取或设置参数`SqlDbType`
>
> `Direction` 获取或设置一个值，表示参数只可输入的参数、只可输出的参数、双向输出的参数还是存储过程返回值参数
>
> `IsNullable` 获取或设置一个值，表示参数是否接受 `null` 值。 `IsNullable`不用于验证参数的值，并且在执行命令时不会阻止发送或接收 `null` 值。
>
> `LocaleId` 获取或设置确定某一特定区域的约定和语言设置的区域设置标识符
>
> `Offset` 获取或设置 `Value` 属性的偏移量
>
> `ParameterName` 获取或设置 `SqIParameter`的名称
>
> `Precision` 获取或设置用于表示 `Value` 属性的最大位数
>
> `Scale` 获取或设置所解析的 `Value` 的小数位数
>
> `Size` 获取或设置列中数据的最大大小 (字节)
>
> `SourceColumn` 获取或设置源列的名称，该源列映射到 `DataSet` 并用于加载或返回 `Value`
>
> `SourceColumnNullMapping` 获取或设置一个值，表示源列是否可以为 `null`。 通过此操作，`SqlCommandBuilder` 能够为可以为 `null` 的列正确地生成 `Update` 语句
>
> `SourceVersion` 获取或设置在加载 `DataRowVersion` 时使用的 `Value` 
>
> `SqlDbType` 获取或设置参数的 `SqIDbType`
>
> `SqlValue `获取或设置作为 SQL 类型的参数的值
>
> `TypeName` 获取或设置表值参数的类型名称
>
> `UdtTypeName` 获取或设置作为参数的表示用户定义类型的 `string`
>
> `Value` 获取或设置参数的值

```xml
<add name="localLogin" connectionString="server=localhost;uid=sa;pwd=0825;database=LoginDB;pooling=true" />
```

```csharp
//无参构造方法的案例

string connStr = ConfigurationManager.ConnectionStrings["localLogin"].ConnectionString;
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

command.Parameters.Add(parameter1);	//往命令中添加参数
command.Parameters.Add(parameter2);

Object? obj = command.ExecuteScalar();	//执行命令，返回一个对象
Console.WriteLine(Convert.ToInt32(obj));

command.Dispose();
connection.Close();
connection.Dispose();
```

数据类型如果合适，`Precision` 则从参数的值 `dbType` 推断出大小

在参数中 `value` 指定时`Object`，`SqlDbType` 将从 `Microsoft.NET Framework` 类型的Object推断

### 8.2 案例1 - 登录

```csharp
string connStr = ConfigurationManager.ConnectionStrings["localLogin"].ConnectionString;
SqlConnection connection = new SqlConnection(connStr);
connection.Open();

string sql = "Select COUNT(*) from LoginData where Account=@account and Pwd=@pwd";
SqlCommand command = new SqlCommand(sql, connection);

SqlParameter parameter1 = new SqlParameter();
parameter1.ParameterName = "account";   //设置参数名
parameter1.SqlDbType = SqlDbType.VarChar;   // 设置类型
parameter1.Size = 20;    //参数大小
parameter1.Value = "zhangsan";   //为参数赋值
            
SqlParameter parameter2 = new SqlParameter();
parameter2.ParameterName = "pwd";   //设置参数名
parameter2.SqlDbType = SqlDbType.VarChar;   // 设置类型
parameter2.Size = 20;    //参数大小
parameter2.Value = "123456789";   //为参数赋值

command.Parameters.Add(parameter1);	//往命令中添加参数
command.Parameters.Add(parameter2);

Object? obj = command.ExecuteScalar();	//执行命令，返回一个对象

if(Convert.ToInt32(obj) > 0)
    Console.WriteLine("登录成功");
else
    Consoel.WriteLine("用户名或密码错误");

command.Dispose();
connection.Close();
connection.Dispose();
```

### 8.3 案例2 - 添加数据

```csharp
// connStr是配置文件中的连接字符串
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
```

### 8.4 案例3 - 更新数据

```csharp
// connStr是配置文件中的连接字符串
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
        
		//批量添加参数对象，传入参数数组
		command.Parameters.AddRange(parameters);
        
		Object obj = command.ExecuteNonQuery();
		if (Convert.ToInt32(obj) > 0)
			Console.WriteLine("修改成功");
		else	
            Console.WriteLine("修改失败");
	}
}
```

### 8.5 案例4 - 查询数据

```csharp
// connStr是配置文件中的连接字符串
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
```

### 8.6 案例5 - 删除数据

```csharp
// connStr是配置文件中的连接字符串
using (SqlConnection conn = new SqlConnection(conStr))
{
    conn.Open();
    string sql = "Delete from StudentInfo where stuId=@stuId";
    using (SqlCommand cmd = new SqlCommand(sql, conn))
    {
        cmd.Parameters.Add(new SqlParameter("stuId", 180325023));
        
        Object obj = cmd.ExecuteNonQuery();	//执行
        Console.WriteLine(Convert.ToInt32(obj));	//查看受影响的行数
    }
}
```

### 8.7 案例6 - 批量删除

```csharp
// 通过循环的操作进行批量的删除
// 1. 构建/传入 需要删除的条件 数组/列表等
// 2. 循环该 数组/列表
// 3. 每次循环 将当前循环到的参数加入到参数对象中
// 4. 每次循环 执行删除的 T-SQL 语句
// 5. ……
// 6. 结束循环，批量删除结束

// connStr是配置文件中的连接字符串
using (SqlConnection conn = new SqlConnection(conStr))
{
    conn.Open();
    string sql = "Delete from Product where Id=@Id";
    using (SqlCommand cmd = new SqlCommand(sql, conn))
    {
        int[] intArr = {180325023, 180325023, 180325023};
        SqlParameter parameter = new("Id", SqlDbType.Int);
        foreach(var item in intArr)
        {
            parameter.Value = item
            cmd.Parameters.Add(parameter);
            cmd.ExecuteNonQuery();	//执行
            
            cmd.Parameters.Clear();	//清除已执行过的内容，避免出现“累积参数”的情况
        } 
    }
}
```

### 8.8 案例7 - 事务

```csharp
// 需要执行多个操作的时候，推荐使用事务

using (SqlConnection conn = new SqlConnection(conStr))
{
	conn.Open();    // 打开连接
	SqlTransaction transaction = conn.BeginTransaction();   // 开启事务

	try
	{
		string sql = "Update StudentInfo set classId=@classId where stuId=@stuId;"
            + "Update StudentScore set "
            + "courseName=@courseName,"
            + "theoryScore=@theoryScore,"
            + "skillScore=@skillScore"
            + " where stuId=@stuId;";    // T-SQL 
		using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))	// 关联事务
		{
            // 实例化参数对象
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
            
			cmd.Parameters.AddRange(parameters);	//添加参数
			cmd.ExecuteNonQuery();	//执行SQL
			transaction.Commit();   //提交事务
		}
	}
	catch (Exception e)	//抓取异常
	{
		transaction.Rollback(); //发生异常时回滚，保证数据的完整性
		throw;
	}
	finally
	{
		transaction.Dispose();  //释放资源
	}            
}
```

### 8.9 案例8 - 存储过程(无输出参数)

```csharp
// 执行数据库里面的存储过程(已有的)
using (SqlConnection conn = new SqlConnection(connStr))
{
	conn.Open();
	using (SqlCommand cmd = new SqlCommand("up_logindata_insert", conn))	//传入存储过程的名称
	{
		//默认是CommandType.Text, 若不修改则视字符串为T-SQL语句
		cmd.CommandType = CommandType.StoredProcedure; 
		
        //参数
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
```

### 8.10 案例9 - 存储过程(有输出参数)

```csharp
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
```

## 9. 在 Web 中使用 ADO

首先需要新建一个 Asp.net core web 项目

在 `Asp.net core Web` 项目中的 `appsetting.json` 文件中进行配置

```json
// appsetting.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "SqlStudent" : "server=localhost;uid=sa;pwd=0825;database=Student;timeout=30;pooling=true;"
  }
}
```

### 9.1 IConfiguration 接口

+ 命名空间 `Microsoft.Extensions.Configuration` 
+ 程序集 `Microsoft.Extensions.Configuration.Abstractions.dll` 

在 `.net web` 项目中，`IConfiguration` 接口在项目启动中就被注册到 `IOC` 容器中，因此只需要在用到的地方注入，即可拿到对象

### 9.2 读取连接字符串

```csharp
// 在 xxController 控制器中读取连接字符

// 注入配置对象（系统在启动时，已在 IOC 容器中注册）
// 只读字段，用于读取配置文件
private readonly IConfiguration _configuration;

// 只读字段，用于存储连接数据库的字符，减少代码的冗余
private readonly string _connectionString;

// xxController 的构造方法
public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
{
	_logger = logger;
	_configuration = configuration;
	_connectionString = _configuration.GetConnectionString("SqlStudent");
}
```

### 9.3 操作数据

```c#
// 在 XXController 控制器中连接数据库
public IActionResult Index()
{
    // 连接数据库
	using SqlConnection conn = new SqlConnection(_connectionString);
	string sql = "Select * from StudentInfo";	// T-SQL语句
	SqlDataAdapter adapter = new(sql,conn);	// 用SqlDataAdapter 适配器获取数据库数据
	DataTable table = new DataTable();	// 实例化 DataTable 对象，用于存储读取到的数据
	adapter.Fill(table);	// 将数据填充 DataTable 对象

	return View(table);	// 向前端返回数据
}
```

```asp
@* 在 xx.cshtml 页面，与上面的控制器对应的页面 *@

@using System.Data
@model System.Data.DataTable

@{
    ViewData["Title"] = "Home Page";
}

<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <p>Learn about <a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>

    <table border="1" class="table table-hover">
        <thead>
            <tr>
                <td>学号</td>
                <td>姓名</td>
                <td>班级</td>
                <td>电话</td>
                <td>性别</td>
                <td>出生日期</td>
            </tr>
        </thead>
        <tbody>
            @foreach (DataRow item in Model.Rows)	@* 将数据循环取出 *@
            {
                <tr>
                    <td>@item["stuId"]</td>
                    <td>@item["stuName"]</td>
                    <td>@item["classId"]</td>
                    <td>@item["stuPhone"]</td>
                    <td>@item["stuSex"]</td>
                    <td>@item["stuBirthday"]</td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

## 10. 封装 DbHelper

在上述的案例中，每次操作数据库都产生了许多重复的代码，出现了代码冗余的现象。

总结上诉案例的操作可分为三类

+ 读取
+ 写入
+ 计数

因此可将这三类封装为三个方法，将 `sql` 语句与字段参数当作方法参数传入，达到重用作用

```csharp
// 这是在 web 项目中新建一个类进行实现

using System.Data;
using System.Data.SqlClient;

namespace WebDemo
{
    /// <summary>
    /// 一个静态的工具类
    /// 用于执行 T-SQL 语句，减少代码的冗余
    /// </summary>
    public static class DbHelper
    {
        private static string _connectionStr;
        
        //静态构造方法
        //只会在第一次访问该类的时候执行一次静态构造
        static DbHelper()
        {
            ConfigurationBuilder configuration = new ConfigurationBuilder();
            //读取配置文件
            IConfigurationRoot config = configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(file =>
            {
                file.Path = "/appsetting.json";
                file.Optional = false;
                file.ReloadOnChange = true;
            }).Build();
            _connectionStr = config.GetConnectionString("SqlStudent");
        }
        
        /// <summary>
        /// 私有化一个连接数据库方法
        /// </summary>
        /// <returns>返回一个实例化的连接对象</returns>
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
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }
        
        /// <summary>
        /// 通常用于执行 增、删、改 操作
        /// </summary>
        /// <param name="tSql">待执行的 T-SQL 命令</param>
        /// <param name="parameters">需要传入的SQL参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(string tSql, params SqlParameter[]? parameters)
        {
            SqlConnection conn = ConnectionAndOpen();
			conn.Open();	// 天王老子来了都不给关上！！！
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
                {
                    Debug.WriteLine("数据库连接已关闭");
                }
            }
        }
    }
}

```

☆：封装好了，但是没有完全封装好！

☆：还有事务、存储过程等可以完善的地方



# End

总算完结了 `ADO.NET` 的知识。本篇文档描述的案例都是在 `Visual Studio 2022` 的编辑器实现。文档中仅仅实现了 `Sql Server` 数据库的连接，或许后续会更新 `MySql` 等其他数据库的连接操作，内容都是大同小异

看到这里，如果觉得这篇文档笔记对你有帮助的话，麻烦您动一下爪子点个星，让更多的人能够留意到这篇笔记

如果条件可以的话，还可以扫下方二维码对我进行支持

<img src="./Images/zhifubao.jpg" alt="支付宝" style="zoom:45%;" /> <img src="./Images/wechat.jpg" alt="微信" style="zoom:40%;" />



