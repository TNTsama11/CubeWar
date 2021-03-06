﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountBase : MonoBase
{
    /// <summary>
    /// 自身需要绑定的事件集合
    /// </summary>
    List<int> list = new List<int>();
    /// <summary>
    /// 绑定一个或多个事件
    /// </summary>
    /// <param name="eventCodes"></param>
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        AccountManager.Instance.Add(list.ToArray(), this);
    }
    /// <summary>
    /// 事件解绑
    /// </summary>
    protected void UnBind()
    {
        AccountManager.Instance.Remove(list.ToArray(), this);
        list.Clear();
    }
    /// <summary>
    /// 销毁时解除事件绑定
    /// </summary>
    public virtual void OnDestroy()
    {
        if (list != null)
        {
            UnBind();
        }
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="areaCode"></param>
    /// <param name="eventCode"></param>
    /// <param name="message"></param>
    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MessageCenter.Instance.Dispatch(areaCode, eventCode, message);
    }
}
