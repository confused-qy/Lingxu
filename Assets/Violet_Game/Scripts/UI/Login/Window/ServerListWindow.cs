using UnityEngine;
using TMPro;

/**
* ServerListWindow.cs
* DESCRIPTION: 服务器列表窗口
*/

public class ServerListWindow : UIBase
{
    [SerializeField, Header("服务器名称")] private TMP_Text _txtServerName;
    [SerializeField, Header("Item父级变换")] private Transform _itemParentTrans;

    private void GenerateServerListItem()
    {
        // TODO: 生成服务器列表Item
    }

    public void OnCloseBtnClicked()
    {
        // 关闭服务器列表窗口
        UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.GameServerWindow);
    }
}
