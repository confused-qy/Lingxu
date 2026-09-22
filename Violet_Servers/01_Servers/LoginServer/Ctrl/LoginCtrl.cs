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
            case NetDefine.CMD_LoginGameServerCode:
                // 处理登录游戏服务器命令
                OnLoginGameServerHandle(serverBase, basePackage);
                break;
            case NetDefine.CMD_CreateRoleCode:
                // 处理创建角色命令
                OnCreateRoleHandle(serverBase, basePackage);
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

        // 验证用户名是否合法
        if (!DataUtils.IsValidUserName(registReq.UserName))
        {
            // 用户名不合法，处理错误逻辑
            serverBase.SendError(basePackage, CmdCode.UserNameIllegal);
            return;
        }

        if (!DataUtils.IsValidMobile(registReq.PhoneNum))
        {
            // 手机号不合法，处理错误逻辑
            serverBase.SendError(basePackage, CmdCode.PhoneNumIllegal);
            return;
        }

        if (registReq.Password.Length < 4 || registReq.Password.Length > 16)
        {
            // 密码不合法，处理错误逻辑
            serverBase.SendError(basePackage, CmdCode.PasswordIllegal);
            return;
        }

        // 所有校验通过后再转发，避免非法请求已被中心服务器执行。
        serverBase._client.SendData(basePackage);

        LogMsg.Info($"Received registration request: user_name={registReq.UserName}, phone_num={registReq.PhoneNum}, password={registReq.Password}");

    }

    private void OnLoginHandle(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理登录命令的具体逻辑
        LoginReq loginReq = LoginReq.Parser.ParseFrom(basePackage.Data);

        // 检查用户是否频繁登录
        long timer = DataUtils.Instance.GetLoginMilliseconds(loginReq.UserName);
        if (timer > 0 && DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timer < 300)
        {
            // 如果上次登录时间小于1秒，直接返回，防止频繁登录
            serverBase.SendError(basePackage, CmdCode.UserOftenLogin);
            return;
        }
        DataUtils.Instance.AddLoginMilliseconds(loginReq.UserName, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
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

    private void OnLoginGameServerHandle(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理登录游戏服务器命令的具体逻辑
        LoginGameServerReq loginGameServerReq = LoginGameServerReq.Parser.ParseFrom(basePackage.Data);
        serverBase._client.SendData(basePackage); // 作为客户端，向中心服务器发送登录游戏服务器请求

        LogMsg.Info($"Received login game server request: account_id={loginGameServerReq.AccountId}, game_server_id={loginGameServerReq.GameServerId}");
    }

    private void OnCreateRoleHandle(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理创建角色命令的具体逻辑
        CreateRoleReq createRoleReq = CreateRoleReq.Parser.ParseFrom(basePackage.Data);
        serverBase._client.SendData(basePackage); // 作为客户端，向中心服务器发送创建角色请求

        LogMsg.Info($"Received create role request: account_id={createRoleReq.AccountId}, nickname={createRoleReq.Nickname}, job_id={createRoleReq.JobId}");
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
            case NetDefine.CMD_LoginGameServerCode:
                // 处理登录游戏服务器响应
                OnLoginGameServerResultHandle(session, basePackage);
                break;
            case NetDefine.CMD_CreateRoleCode:
                // 处理创建角色响应
                OnCreateRoleResultHandle(session, basePackage);
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
        if (registRet.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, registRet.CmdCode);
            return;
        }

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
        if (loginRet.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, loginRet.CmdCode);
            return;
        }

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
        if (serverListRet.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, serverListRet.CmdCode);
            return;
        }

        // 把数据发送给unity端
        if (session != null)
        {
            session.SendData(basePackage);
        }
    }
    
    private void OnLoginGameServerResultHandle(Session session, BasePackage basePackage)
    {
        // 处理登录游戏服务器响应的具体逻辑
        LoginGameServerRet loginGameServerRet = LoginGameServerRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info($"Received login game server result: " + loginGameServerRet.ToString());
        if (loginGameServerRet.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, loginGameServerRet.CmdCode);
            return;
        }

        // 把数据发送给unity端
        if (session != null)
        {
            session.SendData(basePackage);
        }
    }
    
    private void OnCreateRoleResultHandle(Session session, BasePackage basePackage)
    {
        // 处理创建角色响应的具体逻辑
        CreateRoleRet createRoleRet = CreateRoleRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info($"Received create role result: " + createRoleRet.ToString());
        if (createRoleRet.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, createRoleRet.CmdCode);
            return;
        }

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
