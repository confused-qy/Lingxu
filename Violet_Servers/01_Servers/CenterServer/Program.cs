namespace CenterServer;

class Program
{
    static void Main(string[] args)
    {
        NetServer server = new NetServer();
        server.StartServer(NetDefine.IPHost, NetDefine.CenterServerPort);
        Center_LoginCtrl centerLoginCtrl = new Center_LoginCtrl();
        server.RegistCommand(NetDefine.CMD_RegistCode, centerLoginCtrl);

        while (true)
        {
            Thread.Sleep(1); // 防止 CPU 占用过高
        }
    }
}
