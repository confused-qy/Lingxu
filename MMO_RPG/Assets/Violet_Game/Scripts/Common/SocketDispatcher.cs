using Google.Protobuf;
using UnityEngine;
using System.Collections.Generic;

public delegate void OnActionHandler(ByteString data);

public class SocketDispatcher : Singleton<SocketDispatcher>
{
    private Dictionary<int, OnActionHandler> _actionDic = new Dictionary<int, OnActionHandler>();

    // 为指定的协议码添加事件处理器
    public void AddEventHandler(int protoCode, OnActionHandler handler)
    {
        if (!_actionDic.ContainsKey(protoCode))
        {
            _actionDic[protoCode] = handler;
        } 
        else
        {
            _actionDic[protoCode] += handler;
        }
    }
 
    // 为指定的协议码移除事件处理器
    public void RemoveEventHandler(int protoCode, OnActionHandler handler)
    {
        if (_actionDic.ContainsKey(protoCode))
        {
            _actionDic[protoCode] -= handler;
            if (_actionDic[protoCode] == null)
            {
                _actionDic.Remove(protoCode);
            }
        }
    }

    public void DispatchEvent(int protoCode, ByteString data)
    {
        if (_actionDic.ContainsKey(protoCode))
        {
            _actionDic[protoCode]?.Invoke(data);
        }
    }
}
