using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 武器的基类
/// </summary>
public abstract class Arms:MonoBehaviour
{
    public Transform FirePoint;
    public GameObject Bullet;
    public float Speed;
    public int Damage;
    public string Account;
    public float FireInterval;

    abstract public void Fire();

}
