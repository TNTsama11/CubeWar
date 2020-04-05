using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : ManagerBase
{
    public static AnimationManager Instance = null;

	void Awake()
	{
        Instance = this;
	}
	  
}
