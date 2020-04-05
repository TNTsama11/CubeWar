using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : PropsBase
{
    public GameObject Plane;
    public int value;

    void Start()
    {
    }

    void Update()
    {
        if (Camera.main == null)
        {
            return;
        }        
        Plane.transform.LookAt(Camera.main.transform);
    }
}
