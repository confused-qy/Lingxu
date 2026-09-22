using UnityEngine;
using TMPro;
using System;
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
    public Action CloseBtnClickAction;
    public Action<GameServer> ConfirmBtnClickAction;

    public void SetSelectedServer(GameServer gameServer)
    {
        _gameServer = gameServer;
        SetServerName(gameServer?.ServerName ?? "");
    }

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

    private void SetServerName(string serverName)
    {
        _txtServerName.SetText(serverName);
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
                    item.OnItemClickedCB = OnItemClicked;
                    item.OnItemConfirmedCB = OnItemConfirmed;
                }
        }
    }

    public void OnCloseBtnClicked()
    {
        // 关闭服务器列表窗口
        CloseBtnClickAction?.Invoke();
    }

    private GameServer _gameServer;
    private void OnItemClicked(GameServer gameServer)
    {
        SetSelectedServer(gameServer);
    }

    private void OnItemConfirmed(GameServer gameServer)
    {
        ConfirmBtnClickAction?.Invoke(gameServer);
    }

    public void OnConfirmBtnClicked()
    {
        // 确认选择的服务器
        ConfirmBtnClickAction?.Invoke(_gameServer);
    }
}
