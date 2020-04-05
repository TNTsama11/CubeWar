using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformManager : ManagerBase
{
    public static TransformManager Instance = null;

    void Awake()
    {
        Instance = this;
    }


}
