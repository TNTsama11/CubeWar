using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画信息数据类
/// </summary>
public class AnimationMesg 
{
    public string Account;
    public string AnimationName;
    public bool BoolValue;
    public float FloatValue;

    public AnimationMesg()
    {
        this.Account = string.Empty;
        this.AnimationName = string.Empty;
        this.BoolValue = false;
        this.FloatValue = -1;
    }
    public AnimationMesg(string acc,string animationName,bool boolValue=false,float floatValue=-1)
    {
        this.Account = acc;
        this.AnimationName = animationName;
        this.BoolValue = boolValue;
        this.FloatValue = floatValue;
    }
    public void Change(string acc, string animationName, bool boolValue = false, float floatValue = -1)
    {
        this.Account = acc;
        this.AnimationName = animationName;
        this.BoolValue = boolValue;
        this.FloatValue = floatValue;
    }
}
