using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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

    public Action<string, string, bool> LoginBtnClickAction;
    public Action GotoRegistBtnClickAction;

    public void SetRememberedLogin(string account, bool agreement)
    {
        _iptAcct.text = account;
        _togRememberAcct.isOn = !string.IsNullOrEmpty(account);
        _todAgreement.isOn = agreement;
    }

    public void OoGotoRegistBtnClicked()
    {
        GotoRegistBtnClickAction?.Invoke();
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

        // 2. 检查用户协议；通过校验后，把输入交给控制器
        if (!_todAgreement.isOn)
        {
            Debug.Log("请勾选用户协议");
            TipsMgr.Instance.ShowSystemTips("请阅读并勾选用户协议");
            return;
        }

        LoginBtnClickAction?.Invoke(_iptAcct.text, _iptPasd.text, _togRememberAcct.isOn);
    }
}
