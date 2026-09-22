using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;

public class GameServerItem : MonoBehaviour
{
    [SerializeField] private Image _imgRunState;
    [SerializeField] private TMP_Text _texServerName;

    private GameServer _gameServer;

    public Action<GameServer> OnItemClickedCB;
    public Action<GameServer> OnItemConfirmedCB;

    internal void RefreshUI(GameServer gameServer)
    {
        _gameServer = gameServer;
        Color color = Color.white;
        if (gameServer.RunState == 1)
        {
            color = Color.red;
        }
        else if (gameServer.RunState == 2)
        {
            color = Color.yellow;
        }
        else if (gameServer.RunState == 3)
        {
            color = Color.green;
        }
        _imgRunState.color = color;

        string str = "";
        if (gameServer.IsNew == 1)
        {
            str = "(新服)";
        }
        _texServerName.SetText(gameServer.ServerName + str);
    }

    private int _clickCount;
    public void OnItemClicked()
    {
        _clickCount++;
        ResetCount();

        OnItemClickedCB?.Invoke(_gameServer);

        // 双击确认选择服务器
        if (_clickCount >= 2)
        {
            OnItemConfirmedCB?.Invoke(_gameServer);
        }
    }

    private void ResetCount()
    {
        Observable.Timer(TimeSpan.FromMilliseconds(300)).Subscribe(_ => _clickCount = 0);
    }
}
