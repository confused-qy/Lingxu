using UnityEngine;

public class RoleCtrlBase : MonoBehaviour
{
    protected Animator _animator;
    protected CharacterController _characterController;

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

    protected virtual void OnAwake()
    {

    }
    
    protected virtual void OnStart()
    {
        
    }
}
