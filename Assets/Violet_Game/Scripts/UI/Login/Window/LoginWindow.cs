using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
* LoginWindow.cs
* 登录窗口
* DESCRIPTION: 登录窗口，继承自UIBase，封装了UIBase的生命周期函数
*/

public class LoginWindow : UIBase
{
    [SerializeField, Header("账号输入框")]private TMP_InputField _iptAcct;
    [SerializeField, Header("密码输入框")]private TMP_InputField _iptPasd;
    [SerializeField, Header("记住账号")]private Toggle _togRememberAcct;
    [SerializeField, Header("用户协议")]private Toggle _todAgreement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
