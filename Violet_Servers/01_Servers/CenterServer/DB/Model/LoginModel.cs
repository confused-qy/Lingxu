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
}
