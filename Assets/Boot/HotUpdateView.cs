using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotUpdateView : MonoBehaviour
{
    [SerializeField, Tooltip("显示热更新提示的文本组件")] private TMP_Text _texTips;
    [SerializeField, Tooltip("显示热更新进度的滑动条组件")] private Slider _slider;
    [SerializeField, Tooltip("显示热更新进度的文本组件")] private TMP_Text _texProgress;
    [SerializeField, Tooltip("显示热更新进度的图片组件")] private Image _imgProgress;

    private float _slideWidth;
    
    public void Start()
    {
        _slideWidth = _slider.GetComponent<RectTransform>().rect.width;
    }

    public void RefreshUI(float prgs, string prgsTex)
    {
        _slider.value = prgs;
        _texProgress.SetText(prgsTex);

        _imgProgress.transform.localPosition = new Vector3(_slideWidth, 0, 0);
    }

}
