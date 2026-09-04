using UnityEngine;

/**
* GameObjectUtils.cs
* DESCRIPTION: GameObject工具类，封装了GameObject的一些常用操作
*/

public static class GameObjectUtils
{
    public static void Show(this GameObject go, bool isShow = true)
    {
        if (go != null)
        {
            go.SetActive(isShow);
        }
    }
}
