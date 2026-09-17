using UnityEngine;
using System;
using Google.Protobuf;
using System.Threading;


// 网络模块管理类
public class NetSocketMgr : Singleton<NetSocketMgr>
{
    private static NetClient _client;

    public static NetClient Client { get => _client; }

    private static SynchronizationContext synchronizationContext;

    public void Init()
    {
        synchronizationContext = SynchronizationContext.Current;
        // 初始化并连接到服务器
        ConnectServer(NetDefine.IPHost, NetDefine.LoginServerPort);
    }

    public void ConnectServer(string host, int port)
    {
        Disconnect();

        _client = new NetClient(host, port, ClientType.Unity);
        _client.OnReceiveMsg += OnReceiveMsgHandle;

        _client.StartConnect();
    }

    // 处理接收到的消息的回调函数
    public void OnReceiveMsgHandle(int protoCode, ByteString data)
    {
        // 在主线程中分发接收到的消息给相应的事件处理器
        synchronizationContext.Post(_ => SocketDispatcher.Instance.DispatchEvent(protoCode, data), null);
    }
    
    public void Disconnect()
    {
        if (_client != null)
        {
            _client._isNeedReconnect = false;
            _client.Disconnect();
            _client = null;
        }
    }
}
