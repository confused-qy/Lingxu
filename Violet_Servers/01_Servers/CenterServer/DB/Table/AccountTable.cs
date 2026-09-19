using SqlSugar;

// 用户表
[SugarTable("account", TableDescription = "用户表")] // 指定表名
internal class AccountTable
{
    // 自增长：数据库自动生成ID
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    // 用户状态
    [SugarColumn(DefaultValue = "1", IsOnlyIgnoreInsert = true)]
    public byte State { get; set; } = 1;

    // 用户名
    [SugarColumn(Length = 30, IsNullable = false)]
    public string UserName { get; set; }

    // 手机号
    [SugarColumn(Length = 15, IsNullable = false)]
    public string PhoneNum { get; set; }

    // 密码
    [SugarColumn(Length = 100, IsNullable = false)]
    public string Password { get; set; }

    // 上次登录的服务器ID
    public int LastLoginServerId { get; set; }

    // 创建时间
    public DateTime CreateTime { get; set; }

    // 更新时间
    public DateTime UpdateTime { get; set; }
}
