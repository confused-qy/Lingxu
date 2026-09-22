using SqlSugar;


[SugarTable("role", TableDescription = "角色表")]

internal class RoleTable
{
     [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    // 用户状态
    [SugarColumn(DefaultValue = "1", IsOnlyIgnoreInsert = true)]
    public byte State { get; set; } = 1;

    // 账号ID
    public int AccountId { get; set; }

    // 金钱
    public int Money { get; set; }

    // 昵称
    [SugarColumn(Length = 30)]
    public string Nickname { get; set; }

    // 职业ID
    public int JobID { get; set; }

    // 等级
    public int Level { get; set; }

    // 经验
    public int Exp { get; set; }

    // 技能点
    public int SkillUpPoint { get; set; }

    // 最大生命值
    public int MaxHP { get; set; }

    // 当前生命值
    public int CurrHP { get; set; }

    // 最大魔法值
    public int MaxMP { get; set; }
    
    // 当前魔法值
    public int CurrMP { get; set; }

    // 攻击力
    public int Atk { get; set; }

    // 防御力
    public int Def { get; set; }

    // 暴击率
    public int Crit { get; set; }

    // 闪避率
    public int Dodge { get; set; }

    // 命中率
    public int Hit { get; set; }

    // 穿透率
    public int Penet { get; set; }

    // 位置
    [SugarColumn(Length = 50)]
    public string Pos { get; set; }

    // 相机偏移
    [SugarColumn(Length = 50)]
    public string CameraOffset { get; set; }

    // 地图ID
    public int MapId { get; set; }

    // 所属的游戏服务器ID
    public int ServerId { get; set; }

    // 创建时间
    public DateTime CreateTime { get; set; }

    // 更新时间
    public DateTime UpdateTime { get; set; }
}