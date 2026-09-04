using UnityEngine;

/**
* 登录控制器
*/

public class LoginCtrl : CtrlBase
{
    private LoginView _loginView;
    public LoginCtrl(UIBase view) : base(view)
    {
        _loginView = view as LoginView;
        _loginView.InitView();
    }

    // 显示登录视图
    public override void ShowView()
    {
        _loginView.Show();
    }

    // 隐藏登录视图
    public override void HideView()
    {
        _loginView.Show(false);
    }

    // 显示指定窗口
    public void ShowWindow(WindowType windowType)
    {
        _loginView.ShowWindow(windowType);
    }

}
