using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

/**
 * SystemTips.cs
 * DESCRIPTION: 系统提示
 */

public class SystemTips : MonoBehaviour
{
    [SerializeField, Header("提示文本")] private TMP_Text _texMsg;
    [SerializeField, Header("颜色曲线")] private AnimationCurve _colorCurve;
    [SerializeField, Header("移动曲线")] private AnimationCurve _moveCurve;

    public void RefreshUI(string msg)
    {
        _texMsg.SetText(msg);
        _texMsg.DOColor(Color.red, 2f).SetEase(_colorCurve);

        RectTransform rectTrans = transform as RectTransform;
        rectTrans.DOAnchorPosY(rectTrans.anchoredPosition.y + Random.Range(200, 260), 2f).SetEase(_moveCurve);

        // 定时销毁当前对象
        // 响应式编程：Observable 本身被设计成了“发生事情时会发通知”的对象；Subscriber 提前注册，等通知来了就执行。
        Observable.Timer(System.TimeSpan.FromSeconds(3f))
                  .Subscribe(_ => Destroy(gameObject))
                  .AddTo(this);
    }
}
