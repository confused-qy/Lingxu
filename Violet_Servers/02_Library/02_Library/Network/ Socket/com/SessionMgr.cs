using System;
using System.Collections.Generic;
using System.Threading;
public class SessionMgr : Singleton<SessionMgr>
{
    private Dictionary<int, Session> _sessionDic = new Dictionary<int, Session>();
    private int _instanceInter;

    public void AddSession(Session session, int sessionId = -1)
    {
        if (sessionId <= 0 || session == null)
        {
            sessionId = GetInstanceInter();
        }

        if (!_sessionDic.ContainsKey(sessionId))
        {
            session.SessionId = sessionId;
            _sessionDic[sessionId] = session;
        }
    }

    // 删除
    public void RemoveSession(int sessionId)
    {
        if (_sessionDic.ContainsKey(sessionId))
        {
            _sessionDic.Remove(sessionId);
        }
    }

    public int GetInstanceInter()
    {
        return Interlocked.Increment(ref _instanceInter);
    }

    public Session GetSession(int sessionId)
    {
        _sessionDic.TryGetValue(sessionId, out Session session);
        return session;
    }
}