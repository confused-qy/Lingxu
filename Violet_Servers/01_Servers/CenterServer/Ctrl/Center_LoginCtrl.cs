using Protocol;
using Google.Protobuf;

// 中心服务器与登录服务器的控制器
public class Center_LoginCtrl : IContainer
{
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
        LogMsg.Info($"Received registration request: user_name={registReq.UserName}, phone_num={registReq.PhoneNum}");
    }
}