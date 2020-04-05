using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBase : MonoBehaviour
{
    public PropsType PropsType;
    [HideInInspector]
    public int id;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet")
        {
            return;
        }
        Destroy(this.gameObject);
    }
}
