using System;
using System.Net.Sockets;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Protocol;
using System.Threading;


public class NetClient : ServerBase
{
    private string _host;
    private int _port;
    Timer _reconnectTimer;
    private bool _isNeedReconnect = true;

    public NetClient(string ip, int port, ClientType clientType)
    {
        this._host = ip;
        this._port = port;
        this._clientType = clientType;
        this._connState = ConnState.Disconnected;
    }

    // 链接服务端
    public void StartConnect()
    {
        try
        {
            if (_connState != ConnState.Disconnected)
            {
                return;
            }
            if (_socket == null)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            // 开始连接
            _socket.BeginConnect(_host, _port, OnConnectCB, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine("StartConnect error: " + ex.Message);
            Disconnect();
        }
    }

    // 连接回调
    private void OnConnectCB(IAsyncResult ar)
    {
        try
        {
            _socket.EndConnect(ar);
            _connState = ConnState.Connected;

            // 开始接收数据
            BeginReceive();
        }
        catch (Exception ex)
        {
            Console.WriteLine("OnConnectCB error: " + ex.Message);
            _connState = ConnState.Disconnected;
            if (_socket != null)
            {
                Disconnect();
            }
        }
    }

    // 注册命令处理器
    public void RegistCommand(int cmd, IContainer container)
    {
        if (!_cmdDic.ContainsKey(cmd))
        {
            _cmdDic.Add(cmd, container);
        }
    }

    protected override void HandleCommand(BasePackage basePackage)
    {
        if (_clientType == ClientType.Unity)
        {
            OnReceiveMsg?.Invoke(basePackage.ProtoCode, basePackage.Data);
            return;
        }

        IContainer container = _cmdDic[basePackage.ProtoCode];
        if (container == null)
        {
            LogMsg.Info($"No container found for proto code: {basePackage.ProtoCode}");
            return;
        }
        container.OnClientCommand(this, basePackage);
    }

    // 断开连接并设置重连定时器
    protected override void Disconnect()
    {
        _connState = ConnState.Disconnected;
        SetReconnectTimer();

        base.Disconnect();
    }

    // 停止重连定时器
    private void SetReconnectTimer()
    {
        // 设置重连定时器的逻辑
        if (_reconnectTimer == null)
        {
            _reconnectTimer = new Timer((state) =>
            {
                if (_isNeedReconnect && _connState == ConnState.Disconnected)
                {
                    StartConnect();
                }
            }, null, 0, 3000); // 每3秒尝试重连一次
        }

    }

    // 尝试重新连接
    private void ReConn()
    {
        if (_isNeedReconnect)
        {
            StartConnect();
        }
    }

}