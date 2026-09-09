using UnityEngine;

public class WindowBase : MonoBehaviour
{
    public virtual void Show(bool isShow = true)
    {
        gameObject.Show(isShow);
    }
}
