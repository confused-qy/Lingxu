using System;
using UnityEngine;

/**
* 控制器基类
*/

public class CtrlBase : IDisposable
{
    public CtrlBase(UIBase view)
    {
        // 初始化
    }

    public virtual void ShowView()
    {
        // 显示视图
    }

    public virtual void HideView()
    {
        // 隐藏视图
    }

    public virtual void Dispose()
    {
        // 释放资源
    }

}
