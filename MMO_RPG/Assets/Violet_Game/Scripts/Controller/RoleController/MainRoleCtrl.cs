using UnityEngine;

public class MainRoleCtrl : RoleCtrlBase
{
    private PlayerInputCtrl _inputCtrl;
    private float moveSpeed = 10f;
    private float rotationSpeed = 1000f;

    protected override void OnAwake()
    {
        _inputCtrl = GetComponent<PlayerInputCtrl>();
    }
    
    private void Update()
    {
        if (_inputCtrl.Movement != Vector2.zero)
        {
            Vector3 target = new Vector3(_inputCtrl.Movement.x, 0, _inputCtrl.Movement.y);
            // 计算移动目标位置
            target = target * Time.deltaTime * moveSpeed;

            _animator.SetFloat("Movement", 2);

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
