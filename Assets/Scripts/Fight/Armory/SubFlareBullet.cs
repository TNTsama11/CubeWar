using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SubFlareBullet:BulletBase
    {
    public GameObject explode;
    [HideInInspector]
    public GameObject curGameobj;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == curGameobj)
        {
            return;
        }
        if (other.gameObject.tag == "Bullet")
        {
            if (other.GetComponent<BulletBase>().Account == this.Account)
            {
                return;
            }
        }
        else 
        {
            if (other.GetComponent<FightController>() != null && other.GetComponent<FightController>().GetAccount() == this.Account)
            {
                return;
            }
        }
        base.OnTriggerEnter(other);
        Instantiate(explode, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}

