using System;
using System.Collections.Generic;

/// <summary>
/// 客户端处理服务器数据的基类
/// </summary>
    public abstract class HandlerBase
    {
    public abstract void OnReceive(int subCode,object value);
    protected void Dispatch(int areaCode,int eventCode,object message)
    {
        MessageCenter.Instance.Dispatch(areaCode, eventCode, message);
    }
    }

