using System;

public class NetDefine
{
    public const string IPHost = "127.0.0.1"; // 本地IP地址
    public const int CenterServerPort = 10110; // 中心服务器端口
    public const int LoginServerPort = 10120; // 登录服务器端口
    public const int GateServerPort = 10120; // 网关服务器端口
    public const ushort CMD_ErrCode = 10001; // 错误码

    public const ushort CMD_RegistCode = 11010; // 注册码

    public const ushort CMD_LoginCode = 11020; // 登录码

    public const ushort CMD_GetServerListCode = 11030; // 获取服务器列表码

    public const ushort CMD_LoginGameServerCode = 11040; // 登录游戏服务器码

    public const ushort CMD_CreateRoleCode = 11050; // 创建角色码
}

public enum ConnState
{
    Disconnected,
    Connecting,
    Connected,
    Disconnecting
}

public enum ClientType
{
    Unity,
    LoginServer
}
