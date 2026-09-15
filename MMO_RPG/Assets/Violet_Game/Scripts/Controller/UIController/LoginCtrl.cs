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

}
