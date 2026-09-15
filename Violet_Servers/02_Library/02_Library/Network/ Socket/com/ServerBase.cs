using System;
using System.Net.Sockets;
using System.Net;
using Protocol;
using Google.Protobuf;
using System.Collections.Generic;

public class ServerBase
{
    protected ClientType _clientType; // 客户端类型
    public Action<int, ByteString> OnReceiveMsg; // 回调函数，当接收到消息时被调用
    protected Dictionary<int, IContainer> _cmdDic = new Dictionary<int, IContainer>();
    protected byte[] _buffer = new byte[1024 * 4];
    protected Socket _socket;
    protected ConnState _connState;

    protected void BeginReceive()
    {
        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, OnReceiveCB, null);
    }

    // 回调函数，当接收到数据时被调用
    private void OnReceiveCB(IAsyncResult ar)
    {
        try
        {
            Console.WriteLine("OnReceiveCB called");
            // 结束接收，获取接收到的数据长度
            int len = _socket.EndReceive(ar);
            if (len > 0)
            {
                while (true)
                {
                    ushort msgLen = BitConverter.ToUInt16(_buffer, 0); // 无符号16位整数，表示消息长度
                    if (len >= msgLen + 2)
                    {
                        // 拿到了完整的数据
                        byte[] data = NetUtils.Instance.ParseData(_buffer, msgLen);

                        if (data != null)
                        {
                            BasePackage basePackage = BasePackage.Parser.ParseFrom(data);
                            Console.WriteLine("Received proto_code: " + basePackage.ProtoCode);
                            HandleCommand(basePackage);
                        }
                        len -= (msgLen + 2);

                        // 发生了沾包
                        if (len > 0)
                        {
                            Buffer.BlockCopy(_buffer, msgLen + 2, _buffer, 0, len);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // 继续接收数据
                BeginReceive();
            }
        }
        catch (Exception ex)
        {
            Disconnect();
            Console.WriteLine("OnReceiveCB error: " + ex.Message);
            return;
        }
    }

    protected virtual void HandleCommand(BasePackage basePackage) { }

    protected virtual void Disconnect()
    {
        _connState = ConnState.Disconnected;
         
        _socket?.Close();
        _socket = null;
    }

    public void SendData(BasePackage basePackage, int protoCode = -1, ByteString data = null)
    {
        try
        {
            if (protoCode != -1 && data != null)
            {
                basePackage.ProtoCode = protoCode;
                basePackage.Data = data;
            }

            LogMsg.Info($"{_socket.RemoteEndPoint} send data: {basePackage.ToString()}");
            _socket.Send(NetUtils.Instance.MakeData(basePackage.ToByteArray()));
        }
        catch (Exception ex) { Console.WriteLine("SendData error: " + ex.Message); }


    }

    public void SendData(int protoCode = -1, ByteString data = null)
    {
        SendData(new BasePackage(), protoCode, data);
    }

    public void SendError(BasePackage basePackage, CmdCode cmdCode)
    {
        ErrMsg errMsg = new ErrMsg()
        {
            Code = cmdCode,
        };
        SendData(basePackage, NetDefine.CMD_ErrCode, errMsg.ToByteString());
    }
}