using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straight : Arms
{
    public Straight()
    {
        Bullet = Resources.Load("Bullet/StraightBullet") as GameObject;
    }

    public Straight(Transform point, string account,float speed=8f,int damage=-10, float fireInterval = 0.6f)
    {
        Bullet = Resources.Load("Bullet/StraightBullet") as GameObject;
        this.FirePoint = point;
        this.Speed = speed;
        this.Damage = damage;
        this.Account = account;
        this.FireInterval = fireInterval;
    }


    public override void Fire()
    {
        GameObject curBullet = Instantiate(Bullet, FirePoint.position,FirePoint.rotation);
        StraightBullet straightBullet  = curBullet.GetComponent<StraightBullet>();
        straightBullet.Account = this.Account;
        straightBullet.Speed = this.Speed;
        straightBullet.Damage = this.Damage;
    }
}
