using SqlSugar;
using Protocol;
using System;

// 处理登录请求的模型
public class LoginModel
{
    RegistRet ret = new RegistRet();
    SqlSugarClient _db; // 数据库客户端实例
    public LoginModel(SqlSugarClient client)
    {
        _db = client; // 获取数据库客户端实例
    }

    // 处理登录请求的方法
    internal LoginRet LoginAccount(LoginReq loginReq)
    {
        LoginRet ret = new LoginRet();
        AccountTable accountTable = _db.Queryable<AccountTable>()
           .Where(a => a.UserName == loginReq.UserName).First();
        if (accountTable == null)
        {
            ret.CmdCode = CmdCode.AcctNotExist; // 同时设置登录返回的命令码
        }
        else
        {
            if (accountTable.Password != loginReq.Password)
            {
                ret.CmdCode = CmdCode.PasswordError; // 同时设置登录返回的命令码
            }
            else
            {
                if (accountTable.State != 1)
                {
                    ret.CmdCode = CmdCode.AcctDisable; // 同时设置登录返回的命令码
                }
                else
                {
                    // todo：判断账号是否已经登陆
                    GameServerTable gameServerTable = _db.Queryable<GameServerTable>()
                       .Where(g => g.Id == accountTable.LastLoginServerId)
                       .First();

                    if (gameServerTable != null)
                    {
                        ret.GameServer = new GameServer()
                        {
                            ServerId = gameServerTable.Id,
                            ServerName = gameServerTable.ServerName,
                            RunState = gameServerTable.RunState,
                            IsNew = gameServerTable.IsNew,
                            IpHost = gameServerTable.IPHost,
                            Port = gameServerTable.Port
                        };
                    }
                    ret.AccountId = accountTable.Id;
                }
            }
        }
        return ret;
    }

    internal RegistRet RegistAccount(RegistReq registReq)
    {
        ret.CmdCode = CmdCode.Succeed;
        // 1. 判断是否已存在该账号
        var existingAccount = _db.Queryable<AccountTable>()
                                 .Where(a => a.UserName == registReq.UserName)
                                 .ToList();
        if (existingAccount.Count > 0)
        {
            // 账号已存在，处理相应逻辑
            ret.CmdCode = CmdCode.AcctExist; // 设置命令码为账号已存在
        }
        else
        {
            AccountTable accountTable = new AccountTable()
            {
                UserName = registReq.UserName,
                PhoneNum = registReq.PhoneNum,
                Password = registReq.Password,
                LastLoginServerId = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            int id = _db.Insertable(accountTable).ExecuteCommand();
            if (id <= 0)
            {
                ret.CmdCode = CmdCode.ServerError; // 设置命令码为失败
            }

        }
        return ret;
    }

    internal GateServerListRet GetServerList(GateServerListReq req)
    {
        GateServerListRet ret = new GateServerListRet();

        if (req.ServerId == 0)
        {
            List<GameServerTable> gameServerTables = _db.Queryable<GameServerTable>().ToList();
            if (gameServerTables != null && gameServerTables.Count > 0)
            {
                for (int i = 0; i < gameServerTables.Count; i++)
                {
                    GameServer gameServer = new GameServer()
                    {
                        ServerId = gameServerTables[i].Id,
                        ServerName = gameServerTables[i].ServerName,
                        RunState = gameServerTables[i].RunState,
                        IsNew = gameServerTables[i].IsNew,
                        IpHost = gameServerTables[i].IPHost,
                        Port = gameServerTables[i].Port
                    };
                    ret.GameServers.Add(gameServer);
                }
            }
            else
            {
                ret.CmdCode = CmdCode.ServerError; // 设置命令码为失败
            }
        }
        return ret;
    }

    internal LoginGameServerRet LoginGameServer(LoginGameServerReq req)
    {
        LoginGameServerRet ret = new LoginGameServerRet();
        AccountTable accountTable = _db.Queryable<AccountTable>()
           .Where(a => a.Id == req.AccountId)
           .First();

        if (accountTable != null)
        {
            GameServerTable gameServerTable = _db.Queryable<GameServerTable>()
               .Where(g => g.Id == req.GameServerId)
               .First();
            if (gameServerTable != null)
            {
                accountTable.LastLoginServerId = req.GameServerId;
                if (_db.Updateable(accountTable).ExecuteCommand() > 0)
                {
                    // 查询角色信息，当前用户是否已经创建了角色，如果没有创建角色，则返回默认值
                    RoleTable roleTable = _db.Queryable<RoleTable>()
                       .Where(r => r.AccountId == req.AccountId)
                       .First();
                    if (roleTable != null)
                    {
                        ret.CreateRoleInfo = new CreateRoleRet()
                        {
                            RoleId = roleTable.Id,
                            Nickname = roleTable.Nickname,
                            JobId = roleTable.JobID,
                            Level = roleTable.Level
                        };
                    }
                }
                else
                {
                    ret.CmdCode = CmdCode.ServerError; // 更新失败
                }
            }
            else
            {
                ret.CmdCode = CmdCode.ReqParamError; // 游戏服务器不存在
            }
        }
        else
        {
            ret.CmdCode = CmdCode.AcctNotExist; // 账号不存在
        }
        return ret;
    }
    
    internal CreateRoleRet CreateRole(CreateRoleReq req)
    {
        CreateRoleRet ret = new CreateRoleRet();
        RoleTable roleTable = _db.Queryable<RoleTable>()
           .Where(r => r.AccountId == req.AccountId && r.Nickname == req.Nickname)
           .First();
        if (roleTable != null)
        {
            ret.CmdCode = CmdCode.NicknameExist; // 角色昵称已存在
        }
        else
        {
            // 创建角色逻辑
            RoleTable newRole = new RoleTable()
            {
                AccountId = req.AccountId,
                Money = 10000, // 默认是0，晚点改回来
                Nickname = req.Nickname,
                JobID = req.JobId,
                Level = 1, // 默认等级为1
                Exp = 0, // 默认经验为0
                SkillUpPoint = 6, // 用于测试
                MaxHP = 1000,
                CurrHP = 1000,
                MaxMP = 2000,
                CurrMP = 2000,
                Atk = 12,
                Def = 5,
                Crit = 6,
                Dodge = 7,
                Hit = 8,
                Penet = 6,
                Pos = "",
                CameraOffset = "",
                MapId = 1,
                ServerId = req.GameServerId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            
            if (_db.Insertable(newRole).ExecuteCommand() > 0)
            {
                ret.RoleId = newRole.Id;
                ret.Nickname = newRole.Nickname;
                ret.JobId = newRole.JobID;
                ret.Level = newRole.Level;
            }
            else
            {
                ret.CmdCode = CmdCode.ServerError; // 插入失败
            }
            
        }
        return ret;
    }
}
