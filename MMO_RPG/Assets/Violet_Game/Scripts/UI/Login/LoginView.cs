using UnityEngine;
using System;
/**
* LoginView.cs
* 登录视图
* DESCRIPTION: 登录视图，是登录模块所有Window的管理视图类
*/

public class LoginView : UIBase
{
    [SerializeField, Header("登录窗口")] private LoginWindow _loginWindow;
    [SerializeField, Header("注册窗口")] private RegistWindow _registWindow;
    [SerializeField, Header("游戏服务器窗口")] private GameServerWindow _gameServerWindow;
    [SerializeField, Header("服务器列表窗口")] private ServerListWindow _serverListWindow;


    public override void InitView()
    {
        base.InitView();
        windowDic.Add(WindowType.LoginWindow, _loginWindow);
        windowDic.Add(WindowType.RegisterWindow, _registWindow);
        windowDic.Add(WindowType.GameServerWindow, _gameServerWindow);
        windowDic.Add(WindowType.ServerListWindow, _serverListWindow);
    }

    public void RegistGameServerBtnClicked(Action<GameServer> action)
    {
        _gameServerWindow.GameServerBtnClickAction = action;
    }

    public void RegistLoginBtnClicked(Action<string, string, bool> action)
    {
        _loginWindow.LoginBtnClickAction = action;
    }

    public void RegistGotoRegistBtnClicked(Action action)
    {
        _loginWindow.GotoRegistBtnClickAction = action;
    }

    public void RegistRegistBtnClicked(Action<string, string, string> action)
    {
        _registWindow.RegistBtnClickAction = action;
    }

    public void RegistVerifyBtnClicked(Action<string> action)
    {
        _registWindow.VerifyBtnClickAction = action;
    }

    public void RegistBackBtnClicked(Action action)
    {
        _registWindow.BackBtnClickAction = action;
    }

    public void RegistGotoServerListBtnClicked(Action action)
    {
        _gameServerWindow.GotoServerListBtnClickAction = action;
    }

    public void RegistServerListCloseBtnClicked(Action action)
    {
        _serverListWindow.CloseBtnClickAction = action;
    }

    public void RegistServerListConfirmBtnClicked(Action<GameServer> action)
    {
        _serverListWindow.ConfirmBtnClickAction = action;
    }

    public void SetRememberedLogin(string account, bool agreement)
    {
        _loginWindow.SetRememberedLogin(account, agreement);
    }

    public void SetSelectedServer(GameServer gameServer)
    {
        _serverListWindow.SetSelectedServer(gameServer);
    }

}
