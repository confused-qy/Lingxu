using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/**
* ButtonStyle01.cs
* DESCRIPTION: 按钮的动效和特效
*/

public class ButtonStyle01 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Header("按钮的默认缩放")] private float _btnDefaultScale = 1f;
    [SerializeField, Header("按钮的按下缩放")] private float _btnPressedScale = 0.85f;
    [SerializeField, Header("UI特效对象")] GameObject _effectGo;


    public void OnPointerDown(PointerEventData eventData)
    {
        // 按下特效
        transform.DOScale(_btnPressedScale, 0.05f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 弹起特效
        transform.DOScale(_btnDefaultScale, 0.05f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 悬停特效
        if (_effectGo != null)
        {
            _effectGo.Show(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 离开特效
        if (_effectGo != null)
        {
            _effectGo.Show(false);
        }
    }
}
