using System;
using System.Collections.Generic;
using UnityEngine;


    public class EffectMesg
    {

    public GameObject Parent { get; set; }
    public EffectType Effect { get; set; }

    public EffectMesg()
    {

    }
    public EffectMesg(GameObject parent, EffectType type)
    {
        this.Parent = parent;
        this.Effect = type;
    }

    public void Change(GameObject parent, EffectType type)
    {
        this.Parent = parent;
        this.Effect = type;
    }

    }

