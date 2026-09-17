namespace LoginServer;
using System.Threading;
using Protocol;
using System;

class Program
{
    static void Main(string[] args)
    {   
        NetClient client = new NetClient(NetDefine.IPHost, NetDefine.CenterServerPort, ClientType.LoginServer);
        client.StartConnect();

        NetServer server = new NetServer(client);
        server.StartServer(NetDefine.IPHost, NetDefine.LoginServerPort);

        LoginCtrl loginCtrl = new LoginCtrl();
        server.RegistCommand(NetDefine.CMD_RegistCode, loginCtrl);

        client.RegistCommand(NetDefine.CMD_RegistCode, loginCtrl);

        // new Timer(_ =>
        // {
        //     BasePackage basePackage = new BasePackage()
        //     {
        //     };
        //     client.SendData(basePackage);
        // }, null, 5000, Timeout.Infinite);

        while (true)
        {
            Thread.Sleep(1); // 防止 CPU 占用过高
        }
    }
}
