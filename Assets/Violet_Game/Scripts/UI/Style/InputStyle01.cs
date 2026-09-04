using UnityEngine;
using TMPro;
using DG.Tweening;

public class InputStyle01 : MonoBehaviour
{
    [SerializeField, Header("占位符文本")] private TMP_Text _textPlaceholder;
    private float _posY;

    private void Start()
    {
        _posY = _textPlaceholder.rectTransform.anchoredPosition.y;
        TMP_InputField ipt = GetComponent<TMP_InputField>();

        // 默认情况下，如果输入框不为空，则占位符文本向上移动
        if (string.IsNullOrEmpty(ipt.text))
        {
            // DOTween动画，淡入淡出
            _textPlaceholder.rectTransform.DOAnchorPosY(_posY, 0.1f);
        }

        // 当被选中时，向上移动
        ipt.onSelect.AddListener(_ =>
        {
            if (string.IsNullOrEmpty(ipt.text))
            {
                // DOTween动画，淡入淡出
                _textPlaceholder.rectTransform.DOAnchorPosY(_posY + 35, 0.1f);
            }
        });

        // 当不被选中时回到默认位置
        ipt.onDeselect.AddListener(_ =>
        {
            if (string.IsNullOrEmpty(ipt.text))
            {
                // DOTween动画，淡入淡出
                _textPlaceholder.rectTransform.DOAnchorPosY(_posY, 0.1f);
            }
        });

    }
}
