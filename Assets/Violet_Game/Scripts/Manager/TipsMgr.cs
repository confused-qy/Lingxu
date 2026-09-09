using UnityEngine;

/**
 * TipsMgr.cs
 * DESCRIPTION: 提示管理器
 */


public class TipsMgr : Singleton<TipsMgr>
{
    public void ShowSystemTips(string msg)
    {
        ResourceMgr.Instance.LoadPrefabAsync("UIPrefabs/SystemTips", (GameObject go) =>
        {
            // Handle the loaded prefab
            if (go == null)
            {
                Debug.LogError("Failed to load SystemTips prefab.");
                return;
            }
            go.transform.SetParent(GameObject.Find("Canvas").transform);
            go.transform.localPosition = new Vector2(0, 160);
            go.transform.localScale = Vector3.one;

            SystemTips tips = go.GetComponent<SystemTips>();
            if (tips != null)
            {
                tips.RefreshUI(msg);
            }
        });
    }
}
