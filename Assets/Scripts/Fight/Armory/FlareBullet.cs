using System;
using System.Collections.Generic;
using UnityEngine;

    public class FlareBullet:BulletBase
    {
    public GameObject explode;
    public GameObject subBullet;

    protected override void OnTriggerEnter(Collider other)
    {
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
        CreatSubBullet(10,other.gameObject); //子弹分裂
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 生成子子弹
    /// </summary>
    /// <param name="n">个数</param>
    private void CreatSubBullet(int n,GameObject obj)
    {
        GameObject tempFirePpint = new GameObject();
        tempFirePpint.transform.position = this.transform.position;
        tempFirePpint.transform.rotation = this.transform.rotation;
        for(int i = 0; i < n; i++)
        {
            Debug.Log(i);
            GameObject go = Instantiate(subBullet, tempFirePpint.transform.position, tempFirePpint.transform.rotation);
            SubFlareBullet subFlare = go.GetComponent<SubFlareBullet>();
            subFlare.Account = this.Account;
            subFlare.Damage = -2;
            subFlare.Speed = 5f;
            subFlare.curGameobj = obj;
            tempFirePpint.transform.rotation = Quaternion.Euler(tempFirePpint.transform.rotation.eulerAngles+new Vector3(0,360f/n,0));
        }
    }
}

