using SqlSugar;

[SugarTable("game_server", TableDescription = "游戏服务器表")] // 指定表名
internal class GameServerTable
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    [SugarColumn(DefaultValue = "1", IsOnlyIgnoreInsert = true)]
    public byte State { get; set; } = 1;

    [SugarColumn(Length = 30)]
    public string ServerName { get; set; }

    // 运行状态：1. 爆满 2. 拥挤 3. 正常
    public byte RunState { get; set; }

    // 是否为新服
    public byte IsNew { get; set; }

    // IP地址或主机名
    [SugarColumn(Length = 30)]
    public string IPHost { get; set; }

    // 端口号
    public int Port { get; set; }

    // 创建时间
    public DateTime CreateTime { get; set; }

    // 更新时间
    public DateTime UpdateTime { get; set; }

}