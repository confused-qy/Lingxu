using System;
using System.Net.Sockets;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Protocol;
using System.Collections.Generic;

public class Session : ServerBase
{
    public Session(Dictionary<int, IContainer> cmdDic)
    {
        _cmdDic = cmdDic;
    }

    public void ReceiveData(Socket socket)
    {
        _socket = socket;

        // BeginReceive(存到哪里, 从哪里开始存, 最多存多少, 特殊设置, 收到后调用谁, 额外传什么)
        BeginReceive();
    }

    protected override void HandleCommand(BasePackage basePackage)
    {
        IContainer container = _cmdDic[basePackage.ProtoCode];
        if (container == null)
        {
            LogMsg.Info($"No container found for proto code: {basePackage.ProtoCode}");
            return;
        }
        container.OnServerCommand(this, basePackage);
    }
}