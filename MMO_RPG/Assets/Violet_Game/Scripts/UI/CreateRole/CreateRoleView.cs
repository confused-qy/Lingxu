using UnityEngine;
using System;
/**
 * CreateRoleView.cs
 * DESCRIPTION: 创建角色和选择角色的视图
 */

public class CreateRoleView : UIBase
{
    [SerializeField, Header("创建角色窗口")] private CreateRoleWindow _createRoleWindow;
    [SerializeField, Header("选择角色窗口")] private SelectRoleWindow _selectRoleWindow;

    public override void InitView()
    {
        base.InitView();
        windowDic.Add(WindowType.CreateRoleWindow, _createRoleWindow);
        windowDic.Add(WindowType.SelectRoleWindow, _selectRoleWindow);
    }

    public void RegistCreateRoleBtnClickAction(Action<string> action)
    {
        _createRoleWindow.CreateRoleBtnClickAction = action;
    }

    public void RegistStartGameBtnClickAction(Action action)
    {
        _selectRoleWindow.StartGameBtnClickAction = action;
    }
}
