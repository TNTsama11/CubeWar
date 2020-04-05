using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模块的基类
/// 保存自身消息
/// </summary>
public class ManagerBase:MonoBase
{
    /// <summary>
    /// 处理模块自身的消息
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="message"></param>
    public override void Execute(int eventCode, object message)
    {
        if (!dict.ContainsKey(eventCode)) 
        {
            Debug.LogWarning("没有注册过事件" + eventCode);
            return;
        }
        List<MonoBase> list = dict[eventCode]; //给所有绑定这个事件的脚本发送
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Execute(eventCode, message); //调用绑定事件的脚本里的Execute方法
        }
    }

    /// <summary>
    /// 存储消息的事件和与之关联的脚本的字典
    /// </summary>
    private Dictionary<int, List<MonoBase>> dict = new Dictionary<int, List<MonoBase>>();
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="eventCode">事件码</param>
    /// <param name="mono">与事件码关联的脚本</param>
    public void Add(int eventCode,MonoBase mono)
    {
        List<MonoBase> list = null;
        if (!dict.ContainsKey(eventCode))//如果从没有注册过事件
        {
            list = new List<MonoBase>();
            list.Add(mono);
            dict.Add(eventCode, list);
            return;
        }
        list = dict[eventCode];
        list.Add(mono);
    }
    /// <summary>
    /// 注册多个事件
    /// 一个脚本与多个事件关联
    /// </summary>
    /// <param name="eventCodes">事件数组</param>
    /// <param name="mono">关联的脚本</param>
    public void Add(int[] eventCodes,MonoBase mono)
    {
        for (int i = 0; i < eventCodes.Length; i++)
        {
            Add(eventCodes[i], mono);
        }
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    public void Remove(int eventCode,MonoBase mono)
    {
        if (!dict.ContainsKey(eventCode)) //如果没有注册过事件
        {
            Debug.LogWarning("没有注册过事件" + eventCode + "无法移除");
            return;
        }
        List<MonoBase> list = dict[eventCode];
        if (list.Count==1) //如果list里只有一个元素那就移除整个list，为了节约空间。如果还有别的就单独移除
        {
            dict.Remove(eventCode);
        }
        else
        {
            list.Remove(mono);
        }
    }

    /// <summary>
    /// 移除多个事件
    /// </summary>
    /// <param name="eventCodes"></param>
    /// <param name="mono"></param>
    public void Remove(int[]eventCodes,MonoBase mono)
    {
        for (int i = 0; i < eventCodes.Length; i++)
        {
            Remove(eventCodes[i], mono);
        }
    }

}
