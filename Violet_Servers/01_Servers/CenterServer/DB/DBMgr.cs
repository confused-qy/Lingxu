using SqlSugar;

internal class DBMgr : Singleton<DBMgr>
{
    public SqlSugarClient InitDB()
    {
        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=violet_game;User=Violet;Password=D2yuki02;",
            DbType = DbType.MySql,
            IsAutoCloseConnection = true
        });

        db.DbMaintenance.CreateDatabase(); // 创建数据库（如果不存在）

        db.CodeFirst.InitTables(typeof(AccountTable)); // 创建表（如果不存在）

        return db;
    }
}