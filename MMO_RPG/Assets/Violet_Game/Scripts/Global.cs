using UnityEngine;
using YooAsset;


public class Global : MonoBehaviour
{
    public static Global Instance { get; private set; }
    private ResourcePackage _package;
    public ResourcePackage YooPackage => _package;
    public LoginRet LoginInfo { get; set; } // 保存登录信息
    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
        _package = YooAssets.GetPackage("DefaultPackage");
        NetSocketMgr.Instance.Init();
    }

    private void OnApplicationQuit()
    {
        // 在应用程序退出时执行的逻辑
        NetSocketMgr.Instance.Disconnect();
    }
}
