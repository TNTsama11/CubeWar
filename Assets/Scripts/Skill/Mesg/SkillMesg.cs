using System;
using UnityEngine;

public class SkillMesg
{
    public string Account { get; set; }
    public SkillType Type { get; set; }
    public GameObject gameObject { get; set; }

    public SkillMesg()
    {

    }

    public SkillMesg(string acc,SkillType type,GameObject obj)
    {
        this.Account = acc;
        this.Type = type;
        this.gameObject = obj;
    }
}

