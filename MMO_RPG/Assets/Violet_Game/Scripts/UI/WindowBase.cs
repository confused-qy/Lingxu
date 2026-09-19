using UnityEngine;

public class WindowBase : MonoBehaviour
{
    public virtual void Show(bool isShow = true, object obj = null)
    {
        gameObject.Show(isShow);

        if (obj != null)
        {
            RefreshUI(obj);
        }
    }

    public virtual void RefreshUI(object obj)
    {
        
    }
}
