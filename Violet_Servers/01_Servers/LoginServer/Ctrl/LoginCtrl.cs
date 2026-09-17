using Protocol;

public class LoginCtrl : IContainer
{
    public void OnServerCommand(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理与登录服务器的连接
        switch (basePackage.ProtoCode)
        {
            case NetDefine.CMD_RegistCode:
                // 处理注册命令
                OnRegistHandle(serverBase, basePackage);
                break;
            default:
                // 处理其他命令
                break;
        }
    }

    private void OnRegistHandle(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理注册命令的具体逻辑
        RegistReq registReq = RegistReq.Parser.ParseFrom(basePackage.Data);
        serverBase._client.SendData(basePackage); // 作为客户端，向中心服务器发送注册请求
        LogMsg.Info($"Received registration request: user_name={registReq.UserName}, phone_num={registReq.PhoneNum}, password={registReq.Password}");

    }

    public void OnClientCommand(ServerBase serverBase, BasePackage basePackage)
    {
        Session session = SessionMgr.Instance.GetSession(basePackage.UnitySessionId);
        // 登录服务器作为客户端，收到中心服务器的响应
        switch (basePackage.ProtoCode)
        {
            case NetDefine.CMD_RegistCode:
                // 处理注册响应
                OnRegistResultHandle(session, basePackage);
                break;
            default:
                // 处理其他响应
                break;
        }
    }
    private void OnRegistResultHandle(Session session, BasePackage basePackage)
    {
        // 处理注册响应的具体逻辑
        RegistRet registRet = RegistRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info($"Received registration result: " + registRet.ToString());

        // 把数据发送给unity端
        if (session != null)
        {
            session.SendData(basePackage);
        }

    }

    public void OnInit()
    {
        // Initialization code here
    }
}