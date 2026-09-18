using UnityEngine;
using Google.Protobuf;
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

        RegistCommand();
    }

    // 注册命令
    private void RegistCommand()
    {
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_RegistCode, OnRegistHandle);
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_LoginCode, OnLoginHandle);
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
            Debug.Log("Login succeeded.");
            TipsMgr.Instance.ShowSystemTips("登录成功！");
            ShowWindow(WindowType.GameServerWindow);
        }
        else
        {
            Debug.Log("Login failed." + ret.ToString());
            TipsMgr.Instance.ShowSystemTips("登录失败...请重试！");
        }
    }

}
