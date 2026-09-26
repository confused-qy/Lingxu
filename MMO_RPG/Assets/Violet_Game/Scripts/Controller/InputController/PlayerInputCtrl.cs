using UnityEngine;

public class PlayerInputCtrl : MonoBehaviour
{
    private PlayerInput _input;
    public Vector2 Movement
    {
        get => _input.Player.Movement.ReadValue<Vector2>();
    }

    private void Awake()
    {
        _input = new PlayerInput();
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
