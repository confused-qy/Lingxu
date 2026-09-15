using UnityEngine;

/**
 * Singleton.cs
 * DESCRIPTION: 单例基类
 * 单例：整个程序运行期间，这个类只希望有一个对象，并且大家都能方便地访问这个对象。
 */

public class Singleton <T> where T : new()
{
    public static T instance;
    private static object instanceLock = new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock(instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }

}
