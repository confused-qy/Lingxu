using UnityEngine;
/**
* GameServerWindow.cs
* DESCRIPTION: 游戏服务器窗口
*/

public class GameServerWindow : UIBase
{
    public void OnGotoServerListBtnClicked()
    {
        // TODO: 显示服务器列表窗口
        UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.ServerListWindow);
    }

    public void OnGameServerBtnClicked()
    {
        // TODO: 处理游戏服务器按钮点击事件
    }
}
