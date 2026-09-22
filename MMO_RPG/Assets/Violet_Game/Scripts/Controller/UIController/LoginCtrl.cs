using UnityEngine;
using Google.Protobuf;
using Protocol;
/**
* 登录控制器
*/

public class LoginCtrl : CtrlBase
{
    private LoginView _loginView;
    private GameServer _selectedGameServer;
    public LoginCtrl(UIBase view) : base(view)
    {
        _loginView = view as LoginView;
        _loginView.InitView();

        RegistCommand();
        _loginView.SetRememberedLogin(
            PlayerPrefs.GetString("Account", ""),
            PlayerPrefs.GetInt("Agreement", 0) == 1);
    }

    private void OnGameServerBtnClicked(GameServer gameServer)
    {
        if (gameServer == null)
        {
            TipsMgr.Instance.ShowSystemTips("请先选择服务器");
            return;
        }
        _selectedGameServer = gameServer;
        LoginGameServerReq req = new LoginGameServerReq()
        {
            AccountId = Global.Instance.LoginInfo.AccountId,
            GameServerId = gameServer.ServerId
        };

        NetSocketMgr.Client.SendData(NetDefine.CMD_LoginGameServerCode, req.ToByteString());
    }

    // 注册命令
    private void RegistCommand()
    {
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_RegistCode, OnRegistHandle);
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_LoginCode, OnLoginHandle);
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_GetServerListCode, OnGetServerListHandle);
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_LoginGameServerCode, OnLoginGameServerHandle);

        // 注册点击事件
        _loginView.RegistGameServerBtnClicked(OnGameServerBtnClicked);
        _loginView.RegistLoginBtnClicked(OnLoginBtnClicked);
        _loginView.RegistGotoRegistBtnClicked(() => ShowWindow(WindowType.RegisterWindow));
        _loginView.RegistRegistBtnClicked(OnRegistBtnClicked);
        _loginView.RegistVerifyBtnClicked(OnVerifyBtnClicked);
        _loginView.RegistBackBtnClicked(() => ShowWindow(WindowType.LoginWindow));
        _loginView.RegistGotoServerListBtnClicked(OnGotoServerListBtnClicked);
        _loginView.RegistServerListCloseBtnClicked(OnServerListCloseBtnClicked);
        _loginView.RegistServerListConfirmBtnClicked(OnServerListConfirmBtnClicked);
    }

    private void OnLoginBtnClicked(string account, string password, bool rememberAccount)
    {
        PlayerPrefs.SetInt("Agreement", 1);
        PlayerPrefs.SetString("Account", rememberAccount ? account : "");
        LoginReq req = new LoginReq { UserName = account, Password = password };
        NetSocketMgr.Client.SendData(NetDefine.CMD_LoginCode, req.ToByteString());
    }

    private void OnRegistBtnClicked(string account, string mobile, string password)
    {
        RegistReq req = new RegistReq
        {
            UserName = account,
            PhoneNum = mobile,
            Password = password
        };
        NetSocketMgr.Client.SendData(NetDefine.CMD_RegistCode, req.ToByteString());
    }

    private void OnVerifyBtnClicked(string mobile)
    {
        // TODO: 接入验证码服务后，在这里发送请求。
        TipsMgr.Instance.ShowSystemTips("验证码功能暂未开放");
    }

    private void OnGotoServerListBtnClicked()
    {
        GateServerListReq req = new GateServerListReq { ServerId = 0 };
        NetSocketMgr.Client.SendData(NetDefine.CMD_GetServerListCode, req.ToByteString());
    }

    private void OnServerListCloseBtnClicked()
    {
        ShowWindow(WindowType.GameServerWindow, _selectedGameServer);
    }

    private void OnServerListConfirmBtnClicked(GameServer gameServer)
    {
        if (gameServer == null)
        {
            TipsMgr.Instance.ShowSystemTips("请先选择服务器");
            return;
        }
        _selectedGameServer = gameServer;
        ShowWindow(WindowType.GameServerWindow, _selectedGameServer);
    }

    // 处理注册响应
    private void OnRegistHandle(ByteString data)
    {
        RegistRet ret = RegistRet.Parser.ParseFrom(data);

        if (ret != null && ret.CmdCode == CmdCode.Succeed)
        {
            Debug.Log("Registration succeeded.");
            TipsMgr.Instance.ShowSystemTips("注册成功...请登陆！");
            ShowWindow(WindowType.LoginWindow);
        }
        else
        {
            Debug.Log("Registration failed." + ret.ToString());
            TipsMgr.Instance.ShowSystemTips("注册失败...请重试！");
        }
    }

    // 处理登录响应
    private void OnLoginHandle(ByteString data)
    {
        LoginRet ret = LoginRet.Parser.ParseFrom(data);

        if (ret != null && ret.CmdCode == CmdCode.Succeed)
        {
            Debug.Log("Login succeeded." + ret.ToString());
            TipsMgr.Instance.ShowSystemTips("登录成功！");
            Global.Instance.LoginInfo = ret;
            _selectedGameServer = ret.GameServer;
            ShowWindow(WindowType.GameServerWindow, _selectedGameServer);
        }
        else
        {
            Debug.Log("Login failed." + ret.ToString());
            TipsMgr.Instance.ShowSystemTips("登录失败...请重试！");
        }
    }

    // 处理获取服务器列表响应
    private void OnGetServerListHandle(ByteString data)
    {
        GateServerListRet ret = GateServerListRet.Parser.ParseFrom(data);

        if (ret != null && ret.CmdCode == CmdCode.Succeed)
        {
            Debug.Log("Get server list succeeded.");

            _loginView.SetSelectedServer(_selectedGameServer);
            ShowWindow(WindowType.ServerListWindow, ret);
        }
        else
        {
            Debug.Log("Get server list failed." + ret.ToString());
            TipsMgr.Instance.ShowSystemTips("获取服务器列表失败...请重试！");
        }
    }

    // 处理登录游戏服务器响应
    private void OnLoginGameServerHandle(ByteString data)
    {
        LoginGameServerRet ret = LoginGameServerRet.Parser.ParseFrom(data);
        if (ret != null && ret.CmdCode == CmdCode.Succeed)
        {
            Debug.Log("Login game server succeeded." + ret.ToString());
            // 后续创建角色使用本次登录的服务器，而不是账号上次登录的服务器。
            Global.Instance.LoginInfo.GameServer = _selectedGameServer;

            Global.Instance.YooPackage.LoadSceneAsync("Assets/Violet_Game/Scenes/Scene_CreateRole")
            .Completed += handle =>
            {
                UIRoot.Instance.LoginViewCtrl.ShowView(false);

                if (ret.CreateRoleInfo != null)
                {
                    // 1. 是否已经有角色，有角色，跳转选择角色的UI
                    UIRoot.Instance.CreateRoleViewCtrl.ShowWindow(WindowType.SelectRoleWindow, ret.CreateRoleInfo);
                }
                else
                {
                    // 2. 如果还未创建角色，跳转创建角色的UI
                    UIRoot.Instance.CreateRoleViewCtrl.ShowWindow(WindowType.CreateRoleWindow);
                }
            };
        }
    }

}
