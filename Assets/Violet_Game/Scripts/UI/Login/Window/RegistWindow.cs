using UnityEngine;
using TMPro;
using UnityEngine.UI;
/**
* 注册窗口
* DESCRIPTION: 注册窗口，继承自UIBase，封装了UIBase的生命周期函数
*/

public class RegistWindow : UIBase
{
    [SerializeField, Header("账号输入框")]private TMP_InputField _iptAcct;
    [SerializeField, Header("手机号码输入框")] private TMP_InputField _iptMobile;
    [SerializeField, Header("验证码输入框")] private TMP_InputField _iptVerify;
    [SerializeField, Header("密码输入框")] private TMP_InputField _iptPasd;
    [SerializeField, Header("确认密码输入框")] private TMP_InputField _iptSurePasd;

    public void OnRegistBtnClicked()
    {
        // 1. 判断输入框是否为空
        if (string.IsNullOrEmpty(_iptAcct.text))
        {
            Debug.Log("账号输入框为空");
            return;
        }

        if (string.IsNullOrEmpty(_iptMobile.text))
        {
            Debug.Log("手机号码输入框为空");
            return;
        }

        if (string.IsNullOrEmpty(_iptVerify.text))
        {
            Debug.Log("验证码输入框为空");
            return;
        }

        if (string.IsNullOrEmpty(_iptPasd.text))
        {
            Debug.Log("密码输入框为空");
            return;
        }

        if (string.IsNullOrEmpty(_iptSurePasd.text))
        {
            Debug.Log("确认密码输入框为空");
            return;
        }

        // 2. 判断密码和确认密码是否一致
        if (_iptPasd.text != _iptSurePasd.text)
        {
            Debug.Log("两次输入的密码不一致");
            return;
        }

        // 3. 开始注册
        // TODO
        Debug.Log("注册成功");
        Show(false);

    }

    public void OnVerifyBtnClicked()
    {
        // TODO
        Debug.Log("发送验证码成功");
    }

    public void OnBackBtnClicked()
    {
        UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.LoginWindow);
    }
    

}
