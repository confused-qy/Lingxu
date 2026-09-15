using TMPro;
using UnityEngine;

/**
 * CreateRoleWindow.cs
 * DESCRIPTION: 创建角色窗口，目前只有一个角色，默认剑修
 */

public class CreateRoleWindow : WindowBase
{
    [SerializeField, Header("昵称输入框")] private TMP_InputField _iptNickname;

    public void OnCreateRoleBtnClicked()
    {
        // 判断输入框是否为空

        // 获取输入框的昵称合法性

        // 服务器验证，是否创建成功

        // 跳转选择角色的UI界面
        UIRoot.Instance.CreateRoleViewCtrl.ShowWindow(WindowType.SelectRoleWindow);
    }
}
