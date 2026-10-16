using System;
using UniRx;
using UnityEngine;

public class RoleCtrlBase : MonoBehaviour
{
    protected Animator _animator;
    protected CharacterController _characterController;
    protected RoleState _roleState; // 角色状态枚举，表示角色当前的动作状态
    protected int _actionId = Animator.StringToHash("Action"); // 动作参数的哈希值，用于优化性能
    private float _rootMotionSpeed = 1f; // 根运动速度，用于控制角色的移动速度
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void OnAnimatorMove()
    {
        // 使用Animator的位移来移动角色
        Vector3 deltaPosition = _animator.deltaPosition;
        deltaPosition.y = 0; // 忽略垂直位移

        if (_roleState == RoleState.Slider)
        {
            _rootMotionSpeed = 5f; // 滑行时的速度
        }
        else {
            _rootMotionSpeed = 1f; // 其他状态下的速度
        }
        if (deltaPosition != Vector3.zero) // 如果位移足够大，才进行移动
        {
            _characterController.Move(deltaPosition * _rootMotionSpeed);
        }
    }

    private void Update()
    {
        // 检测是否在地面
        IsGround();
        OnUpdate();
    }

    int _atkIndex = 30;
    IDisposable _obs;
    public void ChangeState(RoleState newState)
    {
        _roleState = newState;
        switch (_roleState)
        {
            case RoleState.Idle:
                _animator.SetInteger(_actionId, 1);
                break;
            case RoleState.Run:
                _animator.SetInteger(_actionId, 2);
                break;
            case RoleState.FastRun:
                _animator.SetInteger(_actionId, 3);
                break;
            case RoleState.Jump:
                _animator.SetInteger(_actionId, 21);
                break;
            case RoleState.Slider:
                _animator.SetInteger(_actionId, 41);
                break;
            case RoleState.Attack:
                _atkIndex++;
                if (_obs != null)
                {
                    _obs.Dispose(); // 取消订阅上一个计时器，避免重复触发
                }
                _obs = Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(_ =>
                {
                    _atkIndex = 30;
                });

                _animator.SetInteger(_actionId, _atkIndex);
                if (_atkIndex >= 33)
                {
                    _atkIndex = 30;
                }
                break;
            default:
                break;
        }
    }

    // 角色的垂直速度，用于处理跳跃和下落
    protected float _verticalSpeed;
    // 角色的垂直高度，用于判断是否在地面
    protected float _verticalHeight;
    private void IsGround()
    {
        if (_verticalHeight > transform.position.y && CheckSphereGround())
        {
            _verticalSpeed = 20f; // 角色上升速度
        }
        else if (_verticalHeight < transform.position.y && _verticalHeight != -1000f)
        {
            _verticalHeight = -1000f;
            _verticalSpeed = -20f; // 角色下降速度
        }

        // 过渡值
        _verticalSpeed -= Mathf.Abs(_verticalSpeed * Time.deltaTime) * 1.5f;

        _characterController.Move(transform.up * Time.deltaTime * _verticalSpeed);

        // 检测是否在地面
        if (CheckSphereGround())
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump_Loop"))
            {
                _animator.SetInteger("Action", 23);
            }
             _verticalSpeed = -100f;
            _verticalHeight = transform.position.y;
        }
    }

    protected bool CheckSphereGround()
    {
        // 使用球体检测角色是否在地面上
        Vector3 pos = transform.position + new Vector3(0, 0.1f, 0); // 球体中心位置，稍微抬高一点
        return Physics.CheckSphere(pos, 0.2f, 1 << LayerMask.NameToLayer("Geometry"));
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

    }
    
    protected virtual void OnUpdate()
    {

    }
}
