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
            case NetDefine.CMD_LoginCode:
                // 处理登录命令
                OnLoginHandle(serverBase, basePackage);
                break;
            case NetDefine.CMD_GetServerListCode:
                // 处理获取服务器列表命令
                OnGetServerListHandle(serverBase, basePackage);
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

        // todo: 用户名和密码合法性，还有其他参数
        LogMsg.Info($"Received registration request: user_name={registReq.UserName}, phone_num={registReq.PhoneNum}, password={registReq.Password}");

    }

    private void OnLoginHandle(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理登录命令的具体逻辑
        LoginReq loginReq = LoginReq.Parser.ParseFrom(basePackage.Data);
        serverBase._client.SendData(basePackage); // 作为客户端，向中心服务器发送登录请求

        // todo: 用户名和密码合法性
        LogMsg.Info($"Received login request: user_name={loginReq.UserName}, password={loginReq.Password}");
    }

    private void OnGetServerListHandle(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理获取服务器列表命令的具体逻辑
        GateServerListReq getServerListReq = GateServerListReq.Parser.ParseFrom(basePackage.Data);
        serverBase._client.SendData(basePackage); // 作为客户端，向中心服务器发送获取服务器列表请求

        LogMsg.Info($"Received get server list request");
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
            case NetDefine.CMD_LoginCode:
                // 处理登录响应
                OnLoginResultHandle(session, basePackage);
                break;
            case NetDefine.CMD_GetServerListCode:
                // 处理获取服务器列表响应
                OnGetServerListResultHandle(session, basePackage);
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

    private void OnLoginResultHandle(Session session, BasePackage basePackage)
    {
        // 处理登录响应的具体逻辑
        LoginRet loginRet = LoginRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info($"Received login result: " + loginRet.ToString());
        // 判断loginRet.CmdCode = CmdCode.Succeed; // 设置登录返回的命令码为成功

        // 把数据发送给unity端
        if (session != null)
        {
            session.SendData(basePackage);
        }
    }
    
    private void OnGetServerListResultHandle(Session session, BasePackage basePackage)
    {
        // 处理获取服务器列表响应的具体逻辑
        GateServerListRet serverListRet = GateServerListRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info($"Received get server list result: " + serverListRet.ToString());

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