using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
* SelectRoleWindow.cs
* DESCRIPTION: 选择角色窗口
*/

public class SelectRoleWindow : WindowBase
{
    [SerializeField, Header("角色头像")] private Image _imgHead;

    [SerializeField, Header("角色昵称")] private TMP_Text _texNickname;
    [SerializeField, Header("角色职业等级")] private TMP_Text _texJobLevel;

    public void OnStartGameBtnClicked()
    {
        // TODO: 开始游戏逻辑
    }
}
