using UnityEngine;

public class CreateRoleCtrl : CtrlBase
{
    private CreateRoleView _createRoleView;
    public CreateRoleCtrl(UIBase view) : base(view)
    {
        _createRoleView = view as CreateRoleView;
        _createRoleView.InitView();
    }
}
