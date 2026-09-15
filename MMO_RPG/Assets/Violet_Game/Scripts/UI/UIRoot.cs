using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/**
* UIRoot.cs
* 管理所有UI和控制器
* DESCRIPTION: 对视图进行显示，隐藏，以及数据的操作
*/

public class UIRoot : MonoBehaviour
{
    public static UIRoot Instance;

    [SerializeField, Header("登录视图")] private LoginView _loginView;
    public LoginCtrl LoginViewCtrl { get; private set; }

    [SerializeField, Header("创建角色视图")] private CreateRoleView _createRoleView;
    public CreateRoleCtrl CreateRoleViewCtrl { get; private set; }

    [SerializeField, Header("点击特效")] private ParticleSystem _clickFX;
    
    
    private RectTransform _canvasRectTransform;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        InitCtrl();
    }

    private void Start()
    {
        // 点击特效的位置是相对于父 UI 节点计算的，避免屏幕坐标与世界坐标混用
        _canvasRectTransform = _clickFX.transform.parent as RectTransform;
    }

    private void InitCtrl()
    {
        if (LoginViewCtrl == null) 
        {
            LoginViewCtrl = new LoginCtrl(_loginView);
        }
        if (CreateRoleViewCtrl == null) 
        {
            CreateRoleViewCtrl = new CreateRoleCtrl(_createRoleView);
        }
    }

    private void Update()
    {
        // 检测鼠标点击事件
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 判断点击的是否是UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 播放点击特效
                // ScreenToUI 返回父节点内的局部坐标，应写入 anchoredPosition
                ((RectTransform)_clickFX.transform).anchoredPosition = ScreenToUI(Mouse.current.position.ReadValue());
                _clickFX.Play(true);
            }
        }
    }

    // 把屏幕坐标转换为UI坐标
    public Vector2 ScreenToUI(Vector2 screenPos)
    {
        Vector2 uiPos;
        // Canvas 在父级，不一定就在 _canvasRectTransform 对象上
        Canvas canvas = _canvasRectTransform.GetComponentInParent<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPos, canvas.worldCamera, out uiPos);
        return uiPos;
    }
}
