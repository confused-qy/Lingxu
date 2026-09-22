using TMPro;
using UnityEngine;
using System;

/**
 * CreateRoleWindow.cs
 * DESCRIPTION: 创建角色窗口，目前只有一个角色，默认剑修
 */

public class CreateRoleWindow : WindowBase
{
    [SerializeField, Header("昵称输入框")] private TMP_InputField _iptNickname;

    public Action<string> CreateRoleBtnClickAction;

    public void OnCreateRoleBtnClicked()
    {
        // 判断输入框是否为空
        if (string.IsNullOrEmpty(_iptNickname.text))
        {
            // 昵称为空，提示用户输入昵称
            TipsMgr.Instance.ShowSystemTips("请输入昵称...");
            return;
        }

        // 调用外部注册的点击事件回调
        CreateRoleBtnClickAction?.Invoke(_iptNickname.text);
    }
}
