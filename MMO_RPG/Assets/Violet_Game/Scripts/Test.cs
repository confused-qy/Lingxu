using UnityEngine;
using YooAsset;
using System;
using System.Collections;

public class Test : MonoBehaviour
{
    private ResourcePackage package;
    private void Awake() {

        // 初始化YooAsset
        InitYooAsset();
    }

    /// <summary>
    /// 初始化YooAsset
    /// </summary>
    private void InitYooAsset()
    {

        // 初始化
        YooAssets.Initialize();

        // 设置默认的资源包
        package = YooAssets.CreatePackage("DefaultPackage");

        // 设置该资源包为默认资源包，这样在使用YooAssets的接口加载资源时会优先从默认资源包中获取数据
        YooAssets.SetDefaultPackage(package);

        StartCoroutine(InitPackage());
    }
    
    private IEnumerator InitPackage()
    {
        // 编辑器模式
        EditorSimulateModeParameters editorParameters = new EditorSimulateModeParameters();
        editorParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");
        InitializationOperation operation = package.InitializeAsync(editorParameters);
        gameObject.AddComponent<Global>();

        yield break;
    }
}
