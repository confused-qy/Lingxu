using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(CinemachineOrbitalFollow))]
[RequireComponent(typeof(CinemachineInputAxisController))]
[DefaultExecutionOrder(-100)] // 在 Cinemachine 读取输入前更新右键状态。
public class CameraMgr : MonoBehaviour
{
    [Header("鼠标旋转灵敏度")]
    [SerializeField, Range(0f, 10f), Tooltip("左右旋转幅度，1 为原来的速度，0 为关闭水平旋转。")]
    private float _horizontalSensitivity = 1f;

    [SerializeField, Range(0f, 10f), Tooltip("上下旋转幅度，1 为原来的速度，0 为关闭垂直旋转。")]
    private float _verticalSensitivity = 1f;

    private CinemachineOrbitalFollow _orbitalFollow;
    private CinemachineInputAxisController _inputAxisController;
    private Mouse _mouse;

    private void Awake()
    {
        _orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        _inputAxisController = GetComponent<CinemachineInputAxisController>();
        _mouse = Mouse.current;
        SetRotationInput(false);
        SetOrbites(12f); // 设置初始轨道偏移量为5
    }

    private void Update()
    {
        _mouse = Mouse.current;
        SetRotationInput(Application.isFocused && _mouse != null && _mouse.rightButton.isPressed);

        if (_mouse == null)
            return;

        // 鼠标滚轮缩放
        float scrollValue = _mouse.scroll.ReadValue().y;
        if (scrollValue != 0)
        {
            float offset = _orbitalFollow.Orbits.Top.Height - scrollValue;
            offset = Mathf.Clamp(offset, 5f, 30f); // 限制偏移量在5到30之间
            SetOrbites(offset);
        }
    }

    private void OnDisable()
    {
        SetRotationInput(false);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SetRotationInput(false);
    }

    private void SetRotationInput(bool enabled)
    {
        // 右键旋转时隐藏鼠标，松开、失焦或禁用脚本时恢复显示。
        Cursor.visible = !enabled;

        if (_inputAxisController == null)
            return;

        // 只控制旋转轴，保留缩放轴和现有滚轮逻辑。
        foreach (var controller in _inputAxisController.Controllers)
        {
            if (controller.Owner != _orbitalFollow)
                continue;

            bool horizontal = controller.Name == "Look Orbit X";
            if (!horizontal && controller.Name != "Look Orbit Y")
                continue;

            // 每帧应用 Inspector 设置，保留原有垂直轴的反向输入。
            if (controller.Input != null)
                controller.Input.Gain = horizontal ? _horizontalSensitivity : -_verticalSensitivity;

            if (!enabled && controller.Enabled)
            {
                controller.InputValue = 0;
                if (horizontal)
                    controller.Driver.CancelCurrentInput(ref _orbitalFollow.HorizontalAxis);
                else
                    controller.Driver.CancelCurrentInput(ref _orbitalFollow.VerticalAxis);
            }
            controller.Enabled = enabled;
        }
    }

    [SerializeField] private float _delayTimer = 0.6f;
    public void SetOrbites(float offset)
    {
        // if (_orbitalFollow == null)
        //     _orbitalFollow = GetComponent<CinemachineOrbitalFollow>();

        // _orbitalFollow.OrbitStyle =
        //     CinemachineOrbitalFollow.OrbitStyles.ThreeRing;

        // 上轨道：原来的 m_Orbits[0]
        DOTween.To(() => _orbitalFollow.Orbits.Top.Height, x => _orbitalFollow.Orbits.Top.Height = x, offset, _delayTimer);
        DOTween.To(() => _orbitalFollow.Orbits.Top.Radius, x => _orbitalFollow.Orbits.Top.Radius = x, offset * 0.25f, _delayTimer);

        // 中间轨道：原来的 m_Orbits[1]
        DOTween.To(() => _orbitalFollow.Orbits.Center.Height, x => _orbitalFollow.Orbits.Center.Height = x, offset * 0.5f, _delayTimer);
        DOTween.To(() => _orbitalFollow.Orbits.Center.Radius, x => _orbitalFollow.Orbits.Center.Radius = x, offset * 0.7f, _delayTimer);

        // 下轨道：原来的 m_Orbits[2]
        DOTween.To(() => _orbitalFollow.Orbits.Bottom.Radius, x => _orbitalFollow.Orbits.Bottom.Radius = x, offset * 0.15f, _delayTimer);
    }
}
