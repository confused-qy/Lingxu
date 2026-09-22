using UnityEngine;
using TMPro;
using System;
/**
* 注册窗口
* DESCRIPTION: 注册窗口，继承自UIBase，封装了UIBase的生命周期函数
*/

public class RegistWindow : WindowBase
{
    [SerializeField, Header("账号输入框")]private TMP_InputField _iptAcct;
    [SerializeField, Header("手机号码输入框")] private TMP_InputField _iptMobile;
    [SerializeField, Header("验证码输入框")] private TMP_InputField _iptVerify;
    [SerializeField, Header("密码输入框")] private TMP_InputField _iptPasd;
    [SerializeField, Header("确认密码输入框")] private TMP_InputField _iptSurePasd;

    public Action<string, string, string> RegistBtnClickAction;
    public Action<string> VerifyBtnClickAction;
    public Action BackBtnClickAction;

    public void OnRegistBtnClicked()
    {
        // 1. 判断输入框是否为空
        if (string.IsNullOrEmpty(_iptAcct.text))
        {
            Debug.Log("账号输入框为空");
            TipsMgr.Instance.ShowSystemTips("账号不能为空...");
            return;
        }

        if (string.IsNullOrEmpty(_iptMobile.text))
        {
            Debug.Log("手机号码输入框为空");
            TipsMgr.Instance.ShowSystemTips("手机号码不能为空...");
            return;
        }

        if (string.IsNullOrEmpty(_iptVerify.text))
        {
            Debug.Log("验证码输入框为空");
            TipsMgr.Instance.ShowSystemTips("验证码不能为空...");
            return;
        }

        if (string.IsNullOrEmpty(_iptPasd.text))
        {
            Debug.Log("密码输入框为空");
            TipsMgr.Instance.ShowSystemTips("密码不能为空...");
            return;
        }

        if (string.IsNullOrEmpty(_iptSurePasd.text))
        {
            Debug.Log("确认密码输入框为空");
            TipsMgr.Instance.ShowSystemTips("确认密码不能为空...");
            return;
        }

        // 2. 判断密码和确认密码是否一致
        if (_iptPasd.text != _iptSurePasd.text)
        {
            Debug.Log("两次输入的密码不一致");
            TipsMgr.Instance.ShowSystemTips("两次输入的密码不一致...");
            return;
        }

        RegistBtnClickAction?.Invoke(_iptAcct.text, _iptMobile.text, _iptPasd.text);
    }

    public void OnVerifyBtnClicked()
    {
        VerifyBtnClickAction?.Invoke(_iptMobile.text);
    }

    public void OnBackBtnClicked()
    {
        BackBtnClickAction?.Invoke();
    }


}
