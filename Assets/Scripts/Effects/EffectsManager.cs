using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : ManagerBase
{
    public static EffectsManager Instance = null;

    void Awake()
    {
        Instance = this;
    }
}
