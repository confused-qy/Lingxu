using System;
using System.Net.Sockets;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Protocol;
using System.Collections.Generic;

public class Session : ServerBase
{
    public int SessionId { get; set; }

    public Session(Dictionary<int, IContainer> cmdDic, NetClient client)
    {
        _cmdDic = cmdDic;
        _client = client;
        SessionMgr.Instance.AddSession(this);
    }

    public void ReceiveData(Socket socket)
    {
        _socket = socket;

        // BeginReceive(存到哪里, 从哪里开始存, 最多存多少, 特殊设置, 收到后调用谁, 额外传什么)
        BeginReceive();
    }

    protected override void HandleCommand(BasePackage basePackage)
    {
        if (!_cmdDic.TryGetValue(basePackage.ProtoCode, out IContainer container) || container == null)
        {
            LogMsg.Info($"No container found for proto code: {basePackage.ProtoCode}");
            return;
        }
        if (_client != null)
        {
            if (_client._clientType == ClientType.LoginServer)
            {
                basePackage.UnitySessionId = SessionId;
            }
        }
        container.OnServerCommand(this, basePackage);
    }

    public override void Disconnect()
    {
        LogMsg.Info($"Session disconnected: {_socket?.RemoteEndPoint}");
        SessionMgr.Instance.RemoveSession(SessionId);
        base.Disconnect();
    }
}