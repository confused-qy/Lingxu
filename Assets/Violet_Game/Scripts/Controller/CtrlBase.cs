using System;
using UnityEngine;

/**
* 控制器基类
*/

public class CtrlBase : IDisposable
{
    protected UIBase _view; 
    public CtrlBase(UIBase view)
    {
        // 初始化
        _view = view;
    }

    public virtual void ShowView(bool isShow = true)
    {
        // 显示视图
        _view.Show(isShow);
    }

    public virtual void ShowWindow(WindowType windowType)
    {
        // 显示窗口
        if (!_view.gameObject.activeSelf)
        {
            _view.Show(true);
        }
        _view.ShowWindow(windowType);
    }


    public virtual void Dispose()
    {
        // 释放资源
    }

}
