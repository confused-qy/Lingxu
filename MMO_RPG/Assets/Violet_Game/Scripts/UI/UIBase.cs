using UnityEngine;
using System.Collections.Generic;
/**
* UIBase.cs
* UI基类
* DESCRIPTION: UI基类，继承自MonoBehavior，封装了MonoBehavior的生命周期函数
*/

public class UIBase : MonoBehaviour
{
    /**
    // MonoBehavior生命周期函数
    // 1. 初始化函数

    // 脚本加载时调用，是最先执行的生命周期函数，在脚本的生命周期内只会被调用一次
    private void Awake()
    {
        Debug.Log("Awake");
    }

    // 当脚本被激活的时候调用，每激活一次调用一次该函数（SetActive(true)），在脚本的生命周期内可能会被调用多次
    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    // 在所有Awake()方法调用完成后，并且游戏对象处于激活时调用，在Update之前调用
    private void Start()
    {
        Debug.Log("Start");
    }

    // 2. 更新阶段

    // 每固定时间间隔调用一次，通常是0.02s
    // Edit -> Player Settings -> Time 调整时间间隔
    private void FixedUpdate()
    {
        Debug.Log("FixedUpdate");
    }

    // 每帧调用一次，通常是0.016s
    private void Update()
    {
        Debug.Log("Update");
    }

    // 在所有Update()方法调用完成后调用，也是每帧调用。适合做一些需要在Update之后执行的逻辑，比如摄像机跟随
    private void LateUpdate()
    {
        Debug.Log("LateUpdate");
    }

    // 3. 销毁阶段

    // 当应用程序退出时调用，适合做一些资源释放的操作
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
    }

    // 当对象被禁用时调用
    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }
    
    // 当对象被销毁时调用
    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }
    */

    protected Dictionary<WindowType, WindowBase> windowDic;

    // 留给子类重写的初始化函数，子类可以在该函数中进行UI的初始化操作
    public virtual void InitView()
    {
        windowDic = new Dictionary<WindowType, WindowBase>();
    }

    // 留给子类重写的显示函数，子类可以在该函数中进行UI的显示操作
    public virtual void Show(bool isShow = true)
    {
        gameObject.SetActive(isShow);
    }
    
    public WindowBase GetWindow(WindowType windowType)
    {
        return windowDic[windowType];
    }
    
    public void ShowWindow(WindowType windowType, object obj = null)
    {
        if (!windowDic.ContainsKey(windowType))
        {
            Debug.LogWarning($"Window of type {windowType} does not exist.");
            return;
        }
        // 隐藏所有窗口，显示指定窗口
        foreach (var item in windowDic)
        {
            item.Value.Show(item.Key == windowType, obj);
        }

    }
}
