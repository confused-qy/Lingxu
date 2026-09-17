using Protocol;
using Google.Protobuf;

// 中心服务器与登录服务器的控制器
public class Center_LoginCtrl : IContainer
{
    private LoginModel _loginModel;

    public Center_LoginCtrl(LoginModel loginModel)
    {
        _loginModel = loginModel;
    }

    public void OnClientCommand(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理来自登录服务器的命令
    }

    public void OnServerCommand(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理与登录服务器的连接
        switch (basePackage.ProtoCode)
        {
            case NetDefine.CMD_RegistCode:
                // 处理注册命令
                OnRegistHandle(serverBase, basePackage);
                break;
            default:
                // 处理其他命令
                break;
        }
    }

    public void OnInit()
    {
        // 初始化与登录服务器的连接
    }
    
    private void OnRegistHandle(ServerBase serverBase, BasePackage basePackage)
    {
        // 处理注册命令的具体逻辑
        RegistReq registReq = RegistReq.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info($"Received registration request: " + registReq.ToString());
        RegistRet registRet = _loginModel.RegistAccount(registReq);
        LogMsg.Info($"Registration result: " + registRet.ToString());
        serverBase.SendData(basePackage, basePackage.ProtoCode, registRet.ToByteString());
    }
}