using SqlSugar;
namespace CenterServer;

class Program
{
    static void Main(string[] args)
    {
        NetServer server = new NetServer(null);
        server.StartServer(NetDefine.IPHost, NetDefine.CenterServerPort);

        SqlSugarClient db = DBMgr.Instance.InitDB();
        Center_LoginCtrl centerLoginCtrl = new Center_LoginCtrl(new LoginModel(db));
        server.RegistCommand(NetDefine.CMD_RegistCode, centerLoginCtrl);
        server.RegistCommand(NetDefine.CMD_LoginCode, centerLoginCtrl);
        server.RegistCommand(NetDefine.CMD_GetServerListCode, centerLoginCtrl);

        while (true)
        {
            Thread.Sleep(1); // 防止 CPU 占用过高
        }
    }
}
