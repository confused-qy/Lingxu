public enum WindowType
{
    LoginWindow, // 登录窗口
    RegisterWindow, // 注册窗口
    ForgetPasswordWindow, // 忘记密码窗口
    GameServerWindow, // 游戏服务器窗口
    ServerListWindow, // 服务器列表窗口

    CreateRoleWindow, // 创建角色窗口
    SelectRoleWindow, // 选择角色窗口
}

public enum RoleState
{
    Idle, // 待机状态
    Run, // 奔跑状态
    FastRun, // 冲刺状态
    Jump, // 跳跃状态
    Slider, // 滑行状态
    Attack, // 攻击状态
}