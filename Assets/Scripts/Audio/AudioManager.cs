using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerBase
{
    public static AudioManager Instance = null;

    void Awake()
    {
        Instance = this;
    }

	
}
