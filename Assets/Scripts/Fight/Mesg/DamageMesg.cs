using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMesg 
{
    public string Account { get; set; }
    public int Damage { get; set; }

    public void Change(string acc,int damage)
    {
        this.Account = acc;
        this.Damage = damage;
    }
	
}
