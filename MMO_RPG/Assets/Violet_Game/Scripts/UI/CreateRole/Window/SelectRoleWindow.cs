using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/**
* SelectRoleWindow.cs
* DESCRIPTION: 选择角色窗口
*/

public class SelectRoleWindow : WindowBase
{
    [SerializeField, Header("角色头像")] private Image _imgHead;

    [SerializeField, Header("角色昵称")] private TMP_Text _texNickname;
    [SerializeField, Header("角色职业等级")] private TMP_Text _texJobLevel;

    public override void RefreshUI(object data)
    {
        CreateRoleRet ret = data as CreateRoleRet;
        if (ret != null)
        {
            _imgHead.sprite = Global.Instance.YooPackage.LoadAssetSync<Sprite>("Assets/Violet_Game/UISprites/Icon/icon_head.png").AssetObject as Sprite;
            _texNickname.SetText(ret.Nickname);
            string jobStr = "";
            if (ret.JobId == 1)
            {
                jobStr = "剑修";
            } // ==2,todo: 添加其他职业判断

            _texJobLevel.SetText($"职业：{jobStr}  Lv. {ret.Level}");
        }
    }

    public Action StartGameBtnClickAction;

    public void OnStartGameBtnClicked()
    {
        StartGameBtnClickAction?.Invoke();
    }
}
