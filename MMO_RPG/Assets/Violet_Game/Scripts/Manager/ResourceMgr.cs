using UnityEngine;
using YooAsset;
using System;
using System.Collections.Generic;

public class ResourceMgr : Singleton<ResourceMgr>
{
    // 缓存已加载的预制体
    private Dictionary<string, AssetOperationHandle> prefabDic = new Dictionary<string, AssetOperationHandle>();

    public void LoadPrefabAsync(string path, Action<GameObject> onCompleted)
    {
        if (prefabDic.ContainsKey(path))
        {
            onCompleted?.Invoke(prefabDic[path].InstantiateSync());
            return;
        }
        Global.Instance.YooPackage.LoadAssetAsync($"{ConstDefine.PrefabsPath}{path}").Completed += (AssetOperationHandle handle) =>
        {
            // Handle completion
            GameObject go = handle.InstantiateSync();
            if (!prefabDic.ContainsKey(path))
            {
                prefabDic.Add(path, handle);
            }

            onCompleted?.Invoke(go);
        };
    }
}
