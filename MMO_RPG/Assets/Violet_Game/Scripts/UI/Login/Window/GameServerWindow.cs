using UnityEngine;
using TMPro;
using System;
/**
* GameServerWindow.cs
* DESCRIPTION: 游戏服务器窗口
*/

public class GameServerWindow : WindowBase
{
    [SerializeField, Header("服务器状态")]private TMP_Text _texRunState;
    [SerializeField, Header("服务器名称")] private TMP_Text _texServerName;

    GameServer _gameServer;

    public override void RefreshUI(object obj)
    {
        _gameServer = obj as GameServer;
        if (_gameServer != null)
        {
            Color color = Color.white;
            string runState = "";
            if (_gameServer.RunState == 1)
            {
                color = Color.red;
                runState = "爆满";
            }
            else if (_gameServer.RunState == 2)
            {
                color = Color.yellow;
                runState = "拥挤";
            }
            else if (_gameServer.RunState == 3)
            {
                color = Color.green;
                runState = "正常";
            }
            _texRunState.color = color;
            _texRunState.SetText(runState);

            string str = "";
            if (_gameServer.IsNew == 1)
            {
                str = "(新服)";
            }
            _texServerName.SetText(_gameServer.ServerName + str);
        }
    }

    public Action GotoServerListBtnClickAction;
    public Action<GameServer> GameServerBtnClickAction;

    public void OnGotoServerListBtnClicked()
    {
        GotoServerListBtnClickAction?.Invoke();
    }

    public void OnGameServerBtnClicked()
    {
        GameServerBtnClickAction?.Invoke(_gameServer);
    }
}
