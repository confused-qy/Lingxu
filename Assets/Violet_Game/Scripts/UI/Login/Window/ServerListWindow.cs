using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using YooAsset;

/**
* ServerListWindow.cs
* DESCRIPTION: 服务器列表窗口
*/

public class ServerListWindow : UIBase
{
    [SerializeField, Header("服务器名称")] private TMP_Text _txtServerName;
    [SerializeField, Header("Item父级变换")] private Transform _itemParentTrans;

    private void Start()
    {
        GenerateServerListItem();
    }

    private void GenerateServerListItem()
    {
        // TODO: 生成服务器列表Item

        for (int i = 0; i < 50; i++)
        {
            Global.Instance.YooPackage.LoadAssetAsync("Assets/Violet_Game/Prefabs/UIPrefabs/ServerListItemWidget")
            .Completed += (AssetOperationHandle handle) =>
            {
                GameObject go = handle.InstantiateSync();
                go.transform.SetParent(_itemParentTrans);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
        };
        }
    }

    public void OnCloseBtnClicked()
    {
        // 关闭服务器列表窗口
        UIRoot.Instance.LoginViewCtrl.ShowWindow(WindowType.GameServerWindow);
    }
}
