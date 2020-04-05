using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    float timer = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if (timer > 5f)
        {
            Destroy(this.gameObject);
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
