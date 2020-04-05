using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : ManagerBase
{

    public static SkillManager Instance = null;

    void Awake()
    {
        Instance = this;
    }
}
