using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsRota : MonoBehaviour
{

    public float speed;

    protected void Awake()
    {
        this.transform.position += new Vector3(0, 0.5f, 0);
    }

    protected void Update()
    {
        transform.Rotate(transform.up*speed*Time.deltaTime);
        transform.Rotate(transform.forward * speed * Time.deltaTime);
    }
}
