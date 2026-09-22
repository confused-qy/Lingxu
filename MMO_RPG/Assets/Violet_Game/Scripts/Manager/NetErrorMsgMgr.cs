using UnityEngine;
using Google.Protobuf;

public class NetErrorMsgMgr : Singleton<NetErrorMsgMgr>
{
    public void Init()
    {
        // 注册错误相关信息
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_ErrCode, OnErrorMsgHandle);
    }

    private void OnErrorMsgHandle(ByteString data)
    {
        // 处理错误消息的具体逻辑
        ErrMsg errMsg = ErrMsg.Parser.ParseFrom(data);
        switch (errMsg.Code)
        {
            case CmdCode.Succeed:
                // 请求成功，不处理
                break;
            case CmdCode.AcctExist:
                TipsMgr.Instance.ShowSystemTips("账号已存在...");
                break;
            case CmdCode.ServerError:
                TipsMgr.Instance.ShowSystemTips("服务器错误...");
                break;
            case CmdCode.AcctNotExist:
                TipsMgr.Instance.ShowSystemTips("账号不存在...");
                break;
            case CmdCode.PasswordError:
                TipsMgr.Instance.ShowSystemTips("密码错误...");
                break;
            case CmdCode.AcctDisable:
                TipsMgr.Instance.ShowSystemTips("账号被禁用...");
                break;
            case CmdCode.ReqParamError:
                TipsMgr.Instance.ShowSystemTips("请求参数错误...");
                break;
            case CmdCode.NicknameExist:
                TipsMgr.Instance.ShowSystemTips("昵称已经存在...");
                break;
            case CmdCode.UserNameIllegal:
                TipsMgr.Instance.ShowSystemTips("用户名不合法...");
                break;
            case CmdCode.PhoneNumIllegal:
                TipsMgr.Instance.ShowSystemTips("手机号不合法...");
                break;
            case CmdCode.PasswordIllegal:
                TipsMgr.Instance.ShowSystemTips("密码不合法...");
                break;
            case CmdCode.UserOftenLogin:
                TipsMgr.Instance.ShowSystemTips("用户频繁登录，请稍等...");
                break;
            default:
                TipsMgr.Instance.ShowSystemTips("未知错误...");
                break;
        }
    }
}
