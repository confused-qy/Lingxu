using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
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

    private Dictionary<WindowType, UIBase> windowDic;
    public override void InitView()
    {
        windowDic = new Dictionary<WindowType, UIBase>();
        windowDic.Add(WindowType.LoginWindow, _loginWindow);
        windowDic.Add(WindowType.RegisterWindow, _registWindow);
        windowDic.Add(WindowType.GameServerWindow, _gameServerWindow);
        windowDic.Add(WindowType.ServerListWindow, _serverListWindow);
    }

    public UIBase GetWindow(WindowType windowType)
    {
        return windowDic[windowType];
    }
    
    public void ShowWindow(WindowType windowType)
    {
        // 隐藏所有窗口，显示指定窗口
        foreach (var item in windowDic)
        {
            item.Value.Show(item.Key == windowType);
        }

    }

}
