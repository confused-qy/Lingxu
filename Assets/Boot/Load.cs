using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YooAsset;
using static UnityEngine.Rendering.ReloadAttribute;

/**
 * Title:
 * Description:
 */


public class Load : MonoBehaviour {


    [SerializeField, Header("播放模式")] private EPlayMode _playMode = EPlayMode.EditorSimulateMode;
    [SerializeField, Header("资源系统地址")] private string defaultHostServer;
    [SerializeField, Header("备用地址")] private string fallbackHostServer;

    private ResourcePackage package;

    // 默认缓存的资源数据
    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();


    // AOT元数据程序集名称列表
    public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
    {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",
        //"Assembly-CSharp.dll"
    };

    private void Awake() {

        // 初始化YooAsset
        InitYooAsset();
    }

    /// <summary>
    /// 初始化YooAsset
    /// </summary>
    private void InitYooAsset() {

        // 初始化
        YooAssets.Initialize();

        // 设置默认的资源包
        package = YooAssets.CreatePackage("DefaultPackage");

        // 设置该资源包为默认资源包，这样在使用YooAssets的接口加载资源时会优先从默认资源包中获取数据
        YooAssets.SetDefaultPackage(package);

        StartCoroutine(InitPackage());
    }

    // 初始化资源包
    private IEnumerator InitPackage() {


        InitializationOperation operation = null;

        switch (_playMode) {
            case EPlayMode.EditorSimulateMode:
                // 编辑器模式
                EditorSimulateModeParameters editorParameters = new EditorSimulateModeParameters();
                editorParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");
                operation = package.InitializeAsync(editorParameters);
                break;

            case EPlayMode.HostPlayMode:
                // 联机模式
                HostPlayModeParameters hostParameters = new HostPlayModeParameters();
                hostParameters.BuildinQueryServices = new GameQueryServices(); //̫��ս��DEMO�Ľű��࣬��ϸ��StreamingAssetsHelper
                hostParameters.DeliveryQueryServices = new GameDeliveryQueryServices();
                hostParameters.DecryptionServices = new GameDecryptionServices();
                hostParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                operation = package.InitializeAsync(hostParameters);

                break;
        }
        // 等待初始化完成..
        yield return operation;

        Debug.Log("operation::" + operation.Status);
        if (operation.Status != EOperationStatus.Succeed) {
            Debug.Log($"{operation.Error}");
            yield break;
        }

        // 初始化完成后，获取最新的版本信息
        var versionOperation = package.UpdatePackageVersionAsync();
        yield return versionOperation;
        if (versionOperation.Status != EOperationStatus.Succeed) {
            Debug.LogError("RequestVersion Error::" + versionOperation.Error);
            yield break;
        }

        // 更新资源清单
        var manifestOperation = package.UpdatePackageManifestAsync(versionOperation.PackageVersion);
        yield return manifestOperation;

        // 检查更新结果
        if (manifestOperation.Status != EOperationStatus.Succeed) {
            Debug.LogError("UpdateManifest Error::" + manifestOperation.Error);
            yield break;
        }

        yield return Download();

    }

    /// <summary>
    /// 初始化下载资源
    /// </summary>
    /// <returns></returns>
    private IEnumerator Download() {

        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        // 没有需要下载的资源
        if (downloader.TotalDownloadCount == 0) {
            Debug.Log("没有资源需要下载，直接进入下一步..");
            yield return InitCode();
            yield break;
        }

        // 需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        // 注册回调函数
        downloader.OnDownloadOverCallback = OnDownloadOverCallback; // 下载完成回调
        downloader.OnDownloadErrorCallback = OnDownloadErrorCallback; // 下载错误回调
        downloader.OnDownloadProgressCallback = OnDownloadProgressCallback; // 下载进度回调
        downloader.OnStartDownloadFileCallback = OnStartDownloadFileCallback; // 开始下载文件回调

        downloader.BeginDownload();
        yield return downloader;


        // 下载完成回调
        if (downloader.Status == EOperationStatus.Succeed) {
            // 下载成功
            yield return InitCode();
        }
        else {
            // 下载失败
            Debug.Log("下载失败...");
            yield break;
        }

    }


    /// <summary>
    /// 初始化代码程序集dll
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitCode() {

        var assets = new List<string> {
               "Violet_Game.dll"
        }.Concat(AOTMetaAssemblyNames);

        foreach (var asset in assets) {
            var dllHandle = package.LoadAssetAsync<TextAsset>("Assets/Violet_Game/Dlls/" + asset);
            yield return dllHandle;
            TextAsset textAsset = dllHandle.AssetObject as TextAsset;
            s_assetDatas[asset] = textAsset.bytes;
            Debug.Log($"dll:{asset} size:{textAsset.bytes.Length}");
        }

        LoadMetadataForAOTAssemblies();
#if !UNITY_EDITOR
        // Editor模式下，Violet_Game.dll.bytes已经被自动生成，因此不需要再次加载到内存中。
        var hotAssembly = Assembly.Load(s_assetDatas["Violet_Game.dll"]);
        Debug.Log($"[HotUpdate] Loaded {hotAssembly.FullName}");
        foreach (var typeName in new[] { "UIRoot", "LoginView", "ButtonStyle01" }) {
            var type = hotAssembly.GetType(typeName, true);
            Debug.Log($"[HotUpdate] {type.FullName}, base={type.BaseType}, MonoBehaviour={typeof(MonoBehaviour).IsAssignableFrom(type)}");
        }
#endif

        yield return EnterGame();
    }


    /// <summary>
    /// 进入游戏
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterGame() {

        // 异步加载场景
        SceneOperationHandle handle = package.LoadSceneAsync("Assets/Violet_Game/Scenes/Scene_Login");
        yield return handle;
        Debug.Log($"Scene name is {handle.SceneObject.name}");


    }


    // 下载开始回调
    private void OnStartDownloadFileCallback(string fileName, long sizeBytes) {
        Debug.Log($"开始下载: {fileName}  大小: {sizeBytes / 1024f}KB");
    }

    // 下载进度回调
    private void OnDownloadProgressCallback(int totalDownloadCount, int currentDownloadCount,
        long totalDownloadBytes, long currentDownloadBytes) {

        Debug.Log($"文件总数:: {totalDownloadCount} 已下载文件数::{currentDownloadCount} 总大小::{totalDownloadBytes / 1024.0f / 1024} M " +
           $"  已下载大小::{currentDownloadBytes / 1024}KB");
    }

    // 下载错误回调
    private void OnDownloadErrorCallback(string fileName, string error) {
        Debug.Log($"下载失败::{fileName}  Error::{error}");
    }

    // 下载完成回调
    private void OnDownloadOverCallback(bool isSucceed) {
        Debug.Log("下载" + (isSucceed ? " 成功 " : "失败") + " ....");
    }

    private static void LoadMetadataForAOTAssemblies() {

        /// 注意：这里加载的是AOT dll的元数据，而不是实际的AOT dll字节数据，确保在使用AOT程序集时能够正确调用其方法。
        /// 加载AOT程序集的元数据，确保在使用AOT程序集时能够正确调用其方法。
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyNames) {
            byte[] dllBytes = s_assetDatas[aotDllName];
            // 加载assembly对应的dll元数据，避免被hook成一个aot特有的native方法，从而导致版本不一致的问题。
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }


}

internal class GameDecryptionServices : IDecryptionServices {
    public ulong LoadFromFileOffset(DecryptFileInfo fileInfo) {
        return 32;
    }

    public byte[] LoadFromMemory(DecryptFileInfo fileInfo) {
        throw new NotImplementedException();
    }

    public Stream LoadFromStream(DecryptFileInfo fileInfo) {
        return new FileStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read); ;
    }

    public uint GetManagedReadBufferSize() {
        return 1024;
    }
}

internal class RemoteServices : IRemoteServices {

    private readonly string _defaultHostServer;
    private readonly string _fallbackHostServer;

    public RemoteServices(string defaultHostServer, string fallbackHostServer) {
        _defaultHostServer = defaultHostServer;
        _fallbackHostServer = fallbackHostServer;
    }


    public string GetRemoteFallbackURL(string fileName) {
        return $"{_fallbackHostServer}/{fileName}";
    }

    public string GetRemoteMainURL(string fileName) {
        return $"{_defaultHostServer}/{fileName}";
    }
}

internal class GameDeliveryQueryServices : IDeliveryQueryServices {
    public DeliveryFileInfo GetDeliveryFileInfo(string packageName, string fileName) {
        throw new NotImplementedException();
    }

    public bool QueryDeliveryFiles(string packageName, string fileName) {
        return false;
    }
}



internal class GameQueryServices : IBuildinQueryServices {

    public bool QueryStreamingAssets(string packageName, string fileName) {
        string filePath = Path.Combine(Application.streamingAssetsPath, "yoo", packageName, fileName);
        return File.Exists(filePath);
    }
}
