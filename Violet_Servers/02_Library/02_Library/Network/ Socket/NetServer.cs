using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class NetServer
{
    private Socket _socket;
    private Dictionary<int, IContainer> _cmdDic = new Dictionary<int, IContainer>();

    private NetClient _client;

    public NetServer(NetClient client)
    {
        _client = client;
    }

    public void StartServer(string ip, int port)
    {
        // 三个参数分别是：
        // AddressFamily.InterNetwork 表示使用 IPv4 地址
        // SocketType.Stream 表示使用流式套接字（TCP）
        // ProtocolType.Tcp 表示使用 TCP 协议
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 创建一个终结点，用于绑定到指定的 IP 地址和端口号
        EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        _socket.Bind(endPoint);
        Console.WriteLine("Server bound to " + ip + ":" + port);

        // 开始监听传入的连接请求，参数表示允许的最大挂起连接数
        _socket.Listen(100);
        Console.WriteLine("Server started on " + ip + ":" + port);

        Thread listenThread = new Thread(ListenConnectSocket);
        listenThread.IsBackground = true;
        listenThread.Start();

    }

    // 监听传入的连接请求
    private void ListenConnectSocket()
    {
        _socket.BeginAccept(ClientConnectCB, null);

    }

    private void ClientConnectCB(IAsyncResult ar)
    {
        try
        {
            Socket clientSocket = _socket.EndAccept(ar);
            LogMsg.Info("Client connected: " + clientSocket.RemoteEndPoint);

            // 开始接收客户端发送的数据
            // 使用 Session 类来处理客户端发送的数据
            Session session = new Session(_cmdDic, _client);
            session.ReceiveData(clientSocket);

            // 继续监听新的连接请求
            ListenConnectSocket();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error accepting client: " + ex.Message);
        }
    }

    public void RegistCommand(int cmd, IContainer container)
    {
        if (!_cmdDic.ContainsKey(cmd))
        {
            _cmdDic.Add(cmd, container);
        }
    }

}