using UnityEngine;
using Google.Protobuf;
using Protocol;

public class CreateRoleCtrl : CtrlBase
{
    private CreateRoleView _createRoleView;
    public CreateRoleCtrl(UIBase view) : base(view)
    {
        _createRoleView = view as CreateRoleView;
        _createRoleView.InitView();
        RegistCommand();
    }

    public void RegistCommand()
    {
        // 注册命令逻辑
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_CreateRoleCode, OnCreateRoleHandle);
        _createRoleView.RegistCreateRoleBtnClickAction(OnCreateRoleBtnClicked);
        _createRoleView.RegistStartGameBtnClickAction(OnStartGameBtnClicked);
    }
    private void OnCreateRoleHandle(ByteString data)
    {
        // 处理创建角色的返回逻辑
        CreateRoleRet ret = CreateRoleRet.Parser.ParseFrom(data);
        if (ret != null && ret.CmdCode == CmdCode.Succeed)
        {
            Debug.Log("创建角色成功:" + ret.ToString());
            TipsMgr.Instance.ShowSystemTips("创建角色成功！");
            ShowWindow(WindowType.SelectRoleWindow, ret);
        }
    }
    private void OnCreateRoleBtnClicked(string nickname)
    {
        CreateRoleReq req = new CreateRoleReq()
        {
            AccountId = Global.Instance.LoginInfo.AccountId,
            GameServerId = Global.Instance.LoginInfo.GameServer.ServerId,
            Nickname = nickname,
            JobId = 1 // 默认剑修
        };
        NetSocketMgr.Client.SendData(NetDefine.CMD_CreateRoleCode, req.ToByteString());
    }

    private void OnStartGameBtnClicked()
    {
        // TODO: 接入进入游戏的协议与场景切换。
        TipsMgr.Instance.ShowSystemTips("进入游戏功能暂未开放");
    }

}
