namespace LoginServer;
using System.Threading;
using Protocol;
using System;

class Program
{
    static void Main(string[] args)
    {
        NetClient client = new NetClient("127.0.0.1", 10110, ClientType.LoginServer);
        client.StartConnect();

        new Timer(_ =>
        {
            BasePackage basePackage = new BasePackage()
            {
                ProtoCode = 100
            };
            client.SendData(basePackage);
        }, null, 5000, Timeout.Infinite);

        while (true)
        {
            Thread.Sleep(1); // 防止 CPU 占用过高
        }
    }
}
