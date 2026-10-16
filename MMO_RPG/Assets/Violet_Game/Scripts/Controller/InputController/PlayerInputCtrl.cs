using UnityEngine;
using System;
using UniRx;
using UnityEngine.InputSystem;

public class PlayerInputCtrl : MonoBehaviour
{
    private PlayerInput _input;
    public Action<bool> ShiftKeyIsPressed; // Shift按键状态变化事件
    public Action Jumping; // 跳跃按键状态变化事件
    public Action<string> SkillKeyEvent; // 技能按键事件
    public Vector2 Movement
    {
        get => _input.Player.Movement.ReadValue<Vector2>();
    }

    private void Awake()
    {
        _input = new PlayerInput();
        RegisterInputEvent();
    }

    private void RegisterInputEvent()
    {
        // Shift按键按下时
        _input.Player.Shift.started += ctx =>
        {
            ShiftKeyIsPressed?.Invoke(true);
        };

        // Shift按键松开时
        _input.Player.Shift.canceled += ctx =>
        {
            ShiftKeyIsPressed?.Invoke(false);
        };

        _input.Player.Jump.started += ctx =>
        {
            Jumping?.Invoke();
        };

        Keyboard.current.onTextInput += c =>
        {
            string key = c.ToString().ToUpper();
            switch (key)
            {
                case "Q":
                case "E":
                case "R":
                case "F":
                    SkillKeyEvent?.Invoke(key);
                    break;
                default:
                    break;
            }
        };
    }
    private void OnEnable()
    {
        _input.asset.Enable();
    }

    private void OnDisable()
    {
        _input.asset.Disable();
    }

}
