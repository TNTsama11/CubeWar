using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : BulletBase
{
    public GameObject explode;


    protected override void OnTriggerEnter(Collider other)
    {
        //碰到自己发出的子弹不会消失
        if (other.gameObject.tag == "Bullet")
        {
            if (other.GetComponent<BulletBase>().Account == this.Account)
            {
                return;
            }
        }
        else //碰到自己玩家不会消失
        {
            if (other.GetComponent<FightController>() != null && other.GetComponent<FightController>().GetAccount() == this.Account)
            {
                return;
            }
        }
        base.OnTriggerEnter(other);
        //子弹击中物体效果
        Instantiate(explode, this.transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
}
