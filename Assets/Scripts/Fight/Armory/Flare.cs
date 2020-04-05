using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : Arms
{
    public Flare(Transform point, string account, float speed = 3f, int damage = -10,float fireInterval=1.5f)
    {
        Bullet = Resources.Load("Bullet/FlareBullet") as GameObject;
        this.FirePoint = point;
        this.Speed = speed;
        this.Damage = damage;
        this.Account = account;
        this.FireInterval = fireInterval;
    }

    public override void Fire()
    {
        GameObject curBullet = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
        FlareBullet flareBullet = curBullet.GetComponent<FlareBullet>();
        flareBullet.Account = this.Account;
        flareBullet.Speed = this.Speed;
        flareBullet.Damage = this.Damage;
    }
}
