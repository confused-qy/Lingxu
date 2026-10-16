using System;
using UnityEngine;
using UniRx;

public class MainRoleCtrl : RoleCtrlBase
{
    private PlayerInputCtrl _inputCtrl;
    private float moveSpeed = 10f;
    private float rotationSpeed = 1000f;
    private GhostEffect _ghostEffect;

    protected override void OnAwake()
    {
        _inputCtrl = GetComponent<PlayerInputCtrl>();
        _ghostEffect = GetComponent<GhostEffect>();
        _inputCtrl.ShiftKeyIsPressed += OnShiftKeyChanged;
        _inputCtrl.Jumping += OnJumping;
        _inputCtrl.SkillKeyEvent += SkillKey;
    }

    private void OnShiftKeyChanged(bool isPressed)
    {
        // 处理Shift按键状态变化，如果按下，角色快速跑
        if (isPressed)
        {
            moveSpeed = 18f; // 快速跑
        }
        else
        {
            moveSpeed = 10f; // 正常跑
        }
    }

    private void OnJumping()
    {
        if (_roleState != RoleState.Idle)
        {
            return; // 如果已经在跳跃状态，则不处理新的跳跃请求
        }
        // 起跳动作
        Observable.Timer(TimeSpan.FromMilliseconds(260)).Subscribe(_ =>
        {
            _verticalHeight += 8f; // 增加垂直高度，模拟跳跃
        });

        ChangeState(RoleState.Jump);
    }

    private void SkillKey(string key)
    {
        switch (key)
        {
            case "F":
                ChangeState(RoleState.Slider);
                break;
            case "Q":
                ChangeState(RoleState.Attack);
                break;
        }
    }
    
    override protected void OnUpdate()
    {
        if (_roleState == RoleState.Slider)
        {
            _ghostEffect.CreateGhostEffectObject(Color.white, 0.2f, 0.2f, 0.2f, 0.2f);
        }
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (_roleState == RoleState.Attack)
        {
            return;
        }
        if (_inputCtrl.Movement != Vector2.zero)
        {
            Vector3 target = new Vector3(_inputCtrl.Movement.x, 0, _inputCtrl.Movement.y);
            // 计算移动目标位置
            target = target * Time.deltaTime * moveSpeed;

            if (moveSpeed == 10f)
            {
                _animator.SetFloat("Movement", 2);
            }
           else if (moveSpeed == 18f)
            {
                _animator.SetFloat("Movement", 3);
            }
            // 从本地空间转换为世界空间
            target = Camera.main.transform.TransformDirection(target);
            target.y = 0; // 保持y轴不变，避免角色上下移动

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), rotationSpeed * Time.deltaTime);

            // 角色移动
            _characterController.Move(target);
        }
        else
        {
            _animator.SetFloat("Movement", 0);
        }
    }

}
