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
    public bool _isNeedReconnect = true;

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
        lock (_connectionLock)
        {
            if (_connState != ConnState.Disconnected)
                return;

            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _buffer = new byte[1024 * 4];
                _connState = ConnState.Connecting;
                // 回调保留本次连接，避免旧回调操作新连接。
                _socket.BeginConnect(_host, _port, OnConnectCB, _socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("StartConnect error: " + ex.Message);
                Disconnect();
            }
        }
    }

    private void OnConnectCB(IAsyncResult ar)
    {
        Socket connectingSocket = (Socket)ar.AsyncState;
        lock (_connectionLock)
        {
            try
            {
                connectingSocket.EndConnect(ar);
                if (!ReferenceEquals(connectingSocket, _socket))
                    return;

                _connState = ConnState.Connected;
                _reconnectTimer?.Dispose();
                _reconnectTimer = null;
                BeginReceive();
            }
            catch (Exception ex)
            {
                if (!ReferenceEquals(connectingSocket, _socket))
                    return;

                Console.WriteLine("OnConnectCB error: " + ex.Message);
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

        if (!_cmdDic.TryGetValue(basePackage.ProtoCode, out IContainer container) || container == null)
        {
            LogMsg.Info($"No container found for proto code: {basePackage.ProtoCode}");
            return;
        }
        container.OnClientCommand(this, basePackage);
    }

    // 先清理旧连接，再安排重连；连接状态与Socket由同一把锁保护。
    public override void Disconnect()
    {
        lock (_connectionLock)
        {
            base.Disconnect();
            _reconnectTimer?.Dispose();
            _reconnectTimer = null;
            if (_isNeedReconnect)
            {
                _reconnectTimer = new Timer(Reconnect, null, 3000, Timeout.Infinite);
            }
        }
    }

    private void Reconnect(object state)
    {
        lock (_connectionLock)
        {
            if (_isNeedReconnect && _connState == ConnState.Disconnected)
                StartConnect();
        }
    }
}
