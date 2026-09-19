using SqlSugar;
using Protocol;

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

                    // 设置登录成功的返回信息
                    ret.LastLoginServerId = accountTable.LastLoginServerId;
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
}
