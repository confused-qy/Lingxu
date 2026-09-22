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

        // 创建表（如果不存在）
        db.CodeFirst.InitTables(typeof(AccountTable), typeof(GameServerTable), typeof(RoleTable));
        

        // for (int i = 0; i < 30; i++)
        // {
        //     GameServerTable gameServer = new GameServerTable
        //     {
        //         ServerName = (i + 1).ToString() + "区 九州大陆", 
        //         RunState =1,
        //         IsNew = 1,
        //         IPHost = NetDefine.IPHost,
        //         Port = NetDefine.GateServerPort,
        //         CreateTime = DateTime.Now,
        //         UpdateTime = DateTime.Now
        //     };
        //     db.Insertable(gameServer).ExecuteCommand();
        // }

        return db;
    }
}