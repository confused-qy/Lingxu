using UnityEngine;
using Protocol;
using Google.Protobuf;
/**
* GameServerWindow.cs
* DESCRIPTION: 游戏服务器窗口
*/

public class GameServerWindow : WindowBase
{
    public void OnGotoServerListBtnClicked()
    {
        // TODO: 显示服务器列表窗口
        // UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.ServerListWindow);
        GateServerListReq req = new GateServerListReq()
        {
            ServerId = 0 // 获取所有数据
        };
        NetSocketMgr.Client.SendData(NetDefine.CMD_GetServerListCode, req.ToByteString());
    }

    public void OnGameServerBtnClicked()
    {
        // 服务器请求登录服务器

        // 打开Scene_CreateRole场景
        Global.Instance.YooPackage.LoadSceneAsync("Assets/Violet_Game/Scenes/Scene_CreateRole")
            .Completed += handle =>
            {
                UIRoot.Instance.LoginViewCtrl.ShowView(false);
                // 1. 是否已经有角色，有角色，跳转选择角色的UI

                // 2. 如果还未创建角色，跳转创建角色的UI
                UIRoot.Instance.CreateRoleViewCtrl.ShowWindow(WindowType.CreateRoleWindow);
            };

    }
}
