using UnityEngine;
using YooAsset;


public class Global : MonoBehaviour
{
    public static Global Instance { get; private set; }
    private ResourcePackage _package;
    public ResourcePackage YooPackage => _package;
    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
        _package = YooAssets.GetPackage("DefaultPackage");
    }
}
