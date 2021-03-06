﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBase : MonoBase
{
    /// <summary>
    /// 自身需要绑定的事件集合
    /// </summary>
    List<int> list = new List<int>();
    /// <summary>
    /// 绑定一个或多个事件
    /// 绑定事件后自身会被记录到字典里
    /// </summary>
    /// <param name="eventCodes"></param>
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        TransformManager.Instance.Add(list.ToArray(), this);
    }
    /// <summary>
    /// 事件解绑
    /// </summary>
    protected void UnBind()
    {
        TransformManager.Instance.Remove(list.ToArray(), this);
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
    public void Dispatch(int areaCode,int eventCode,object message)
    {
        MessageCenter.Instance.Dispatch(areaCode,eventCode,message);
    }
}
