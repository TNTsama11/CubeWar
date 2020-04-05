using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : ManagerBase
{
    public static AccountManager Instance = null;

    void Awake()
    {
        Instance = this;
    }


}
