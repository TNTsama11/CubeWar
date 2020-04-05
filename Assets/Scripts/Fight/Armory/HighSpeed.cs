using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSpeed : Arms
{

    public HighSpeed()
    {
        Bullet = Resources.Load("Bullet/HighSpeedBullet") as GameObject;
    }

    public HighSpeed(Transform point, string account, float speed = 12f, int damage = -10, float fireInterval = 0.4f)
    {
        Bullet = Resources.Load("Bullet/HighSpeedBullet") as GameObject;
        this.FirePoint = point;
        this.Speed = speed;
        this.Damage = damage;
        this.Account = account;
        this.FireInterval = fireInterval;
    }

    public override void Fire()
    {
        GameObject curBullet = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
        HighSpeedBullet highSpeedBullet = curBullet.GetComponent<HighSpeedBullet>();
        highSpeedBullet.Account = this.Account;
        highSpeedBullet.Speed = this.Speed;
        highSpeedBullet.Damage = this.Damage;
    }
}
