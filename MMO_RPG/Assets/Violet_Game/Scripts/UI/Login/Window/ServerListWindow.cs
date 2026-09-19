using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using YooAsset;
using Google.Protobuf.Collections;
using System.Linq;

/**
* ServerListWindow.cs
* DESCRIPTION: 服务器列表窗口
*/

public class ServerListWindow : WindowBase
{
    [SerializeField, Header("服务器名称")] private TMP_Text _txtServerName;
    [SerializeField, Header("Item父级变换")] private Transform _itemParentTrans;

    private RepeatedField<GameServer> _gameServers;

    public override void RefreshUI(object obj)
    {
        GateServerListRet ret = obj as GateServerListRet;
        if (ret != null && ret.GameServers != null && ret.GameServers.Count > 0)
        {
            // 如果服务器列表没有变化，则不需要重新生成列表
            if (_gameServers != null && _gameServers.SequenceEqual(ret.GameServers))
            {
                return;
            }
            _gameServers = ret.GameServers;
            GenerateServerListItem();
        }
    }

    // 生成服务器列表项
    private async void GenerateServerListItem()
    {
        AssetOperationHandle handle = Global.Instance.YooPackage.LoadAssetAsync("Assets/Violet_Game/Prefabs/UIPrefabs/ServerListItemWidget");
        await handle.Task;
        for (int i = 0; i < _gameServers.Count; i++)
        {
                GameObject go = handle.InstantiateSync();
                go.transform.SetParent(_itemParentTrans);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;

                GameServerItem item = go.GetComponent<GameServerItem>();
                if (item != null)
                {
                    item.RefreshUI(_gameServers[i]);
                }
        }
    }

    public void OnCloseBtnClicked()
    {
        // 关闭服务器列表窗口
        UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.GameServerWindow);
    }
}
