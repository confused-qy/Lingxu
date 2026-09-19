using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameServerItem : MonoBehaviour
{
    [SerializeField] private Image _imgRunState;
    [SerializeField] private TMP_Text _texServerName;

    internal void RefreshUI(GameServer gameServer)
    {
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
            str = "（新服）";
        }
        _texServerName.SetText(str + gameServer.ServerName + str);
    }
}
