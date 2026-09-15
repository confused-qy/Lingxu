using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
* LoginWindow.cs
* 登录窗口
* DESCRIPTION: 登录窗口，继承自UIBase，封装了UIBase的生命周期函数
*/

public class LoginWindow : WindowBase
{
    [SerializeField, Header("账号输入框")]private TMP_InputField _iptAcct;
    [SerializeField, Header("密码输入框")]private TMP_InputField _iptPasd;
    [SerializeField, Header("记住账号")]private Toggle _togRememberAcct;
    [SerializeField, Header("用户协议")] private Toggle _todAgreement;
    
    private void Awake()
    {
        // 1. 判断本地是否保存了账号，如果有，则显示在输入框中
        string acct = PlayerPrefs.GetString("Account", "");
        if (!string.IsNullOrEmpty(acct))
        {
            _iptAcct.text = acct;
            _togRememberAcct.isOn = true;
        }

        // 2. 判断本地是否保存了用户协议，如果有，则勾选
        int agreement = PlayerPrefs.GetInt("Agreement", 0);
        if (agreement == 1)
        {
            _todAgreement.isOn = true;
        }
    }

    public void OoGotoRegistBtnClicked()
    {
        UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.RegisterWindow);
    }

    public void OnLoginBtnClicked()
    {
        // 1. 判断输入框是否为空
        if (string.IsNullOrEmpty(_iptAcct.text))
        {
            Debug.Log("账号输入框为空");
            TipsMgr.Instance.ShowSystemTips("账号不能为空...");
            return;
        }

        if (string.IsNullOrEmpty(_iptPasd.text))
        {
            Debug.Log("密码输入框为空");
            TipsMgr.Instance.ShowSystemTips("密码不能为空...");
            return;
        }

        // 2. 判断是否勾选了用户协议，如果有勾选，则保存在本地
        if (!_todAgreement.isOn)
        {
            Debug.Log("请勾选用户协议");
            TipsMgr.Instance.ShowSystemTips("请阅读并勾选用户协议");
            return;
        }

        PlayerPrefs.SetInt("Agreement", 1);

        // 3. 判断是否勾选了记住账号，如果有勾选，则保存在本地
        if (_togRememberAcct.isOn)
        {
            PlayerPrefs.SetString("Account", _iptAcct.text);
        }
        else
        {
            PlayerPrefs.SetString("Account", "");
        }

        // PlayerPrefs: Unity提供的一个简单的本地存储系统，可以用来保存一些简单的数据，比如玩家的设置，游戏进度等。它会将数据保存在本地的注册表或者配置文件中，数据类型包括int、float、string等。

        // 4. 服务器验证，只有服务器验证通过了才可以登录
        // TODO
        Debug.Log("登录成功");
        UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.GameServerWindow);
    }
}
