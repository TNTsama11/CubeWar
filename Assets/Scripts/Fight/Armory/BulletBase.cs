using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [HideInInspector]
    public string Account;
    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public int Damage;
    public AudioClip HitAudio;
    public AudioClip FlyAudio;

    private Rigidbody rig;

    protected void Awake()
    {
        rig = this.GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        if (FlyAudio != null)
        {
            AudioSource.PlayClipAtPoint(FlyAudio, this.transform.position, 1);
        }
    }
    protected virtual void Update()
    {
        Move();
        if(Mathf.Abs(this.transform.position.x)>50|| Mathf.Abs(this.transform.position.z) > 50)
        {
            Destroy(this.gameObject);
        }
    }


    private void Move()
    {
        // rig.AddForce(this.transform.forward*Speed, ForceMode.Impulse);
        this.transform.Translate(this.transform.forward * Speed * Time.deltaTime,Space.World);
        
    }

    protected virtual void OnTriggerEnter(Collider other) //.-..---...-.
    {        
        //玩家碰到子弹后会获取子弹上的这个脚本然后扣血并判断伤害来源
        //Debug.Log(other.name);
        FightController fightController = other.GetComponent<FightController>();
        if (fightController != null)
        {
            if (fightController.GetAccount() == this.Account)
            {
                return;
            }
        }
         AudioSource.PlayClipAtPoint(HitAudio, this.transform.position,1);
        //子类中重写这个方法 实现特殊效果之后再调用Destroy();
    }



}
