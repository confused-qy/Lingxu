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

    private Dictionary<WindowType, UIBase> windowDic;
    public override void InitView()
    {
        windowDic = new Dictionary<WindowType, UIBase>();
        windowDic.Add(WindowType.LoginWindow, _loginWindow);
        windowDic.Add(WindowType.RegisterWindow, _registWindow);
    }

    public UIBase GetWindow(WindowType windowType)
    {
        return windowDic[windowType];
    }
    
    public void ShowWindow(WindowType windowType)
    {
        // 1. 隐藏所有窗口
        foreach (var window in windowDic.Values)
        {
            window.Show(false);
        }
        // 2. 显示指定窗口
        windowDic[windowType].Show();

    }

}
