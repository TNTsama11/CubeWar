using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : ManagerBase
{
    public static FightManager Instance = null;
	
	void Awake()
	{
        Instance = this;
	}
	

}
