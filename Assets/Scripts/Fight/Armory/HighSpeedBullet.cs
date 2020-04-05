using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HighSpeedBullet:BulletBase
    {

    public GameObject explode;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Instantiate(explode, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}

