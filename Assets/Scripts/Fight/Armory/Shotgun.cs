using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Arms
{

    public Shotgun(Transform point, string account, float speed = 5f, int damage = -5, float fireInterval = 1f)
    {
        Bullet = Resources.Load("Bullet/ShotgunBullet") as GameObject;
        this.FirePoint = point;
        this.Speed = speed;
        this.Damage = damage;
        this.Account = account;
        this.FireInterval = fireInterval;
    }

    public override void Fire()
    {
        GameObject tempFirePoint = new GameObject();
        tempFirePoint.transform.position = FirePoint.position;
        tempFirePoint.transform.rotation = Quaternion.Euler((FirePoint.rotation.eulerAngles - new Vector3(0, 10, 0)));       
        for(int i=0; i < 3; i++)
        {
            GameObject curBullet = Instantiate(Bullet, tempFirePoint.transform.position, tempFirePoint.transform.rotation);
            ShotgunBullet shotgunBullet = curBullet.GetComponent<ShotgunBullet>();
            shotgunBullet.Account = this.Account;
            shotgunBullet.Speed = this.Speed;
            shotgunBullet.Damage = this.Damage;
            tempFirePoint.transform.rotation = Quaternion.Euler((tempFirePoint.transform.rotation.eulerAngles + new Vector3(0, 10, 0)));
        }
    }

    
}
