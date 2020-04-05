using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireText : MonoBehaviour
{
    public Transform firePoint;

    void Start()
    {
        Flare flare = new Flare(firePoint, null);
        StartCoroutine(Fire(flare));
    }
    IEnumerator Fire(Flare flare)
    {

        while (true)
        {
            yield return new WaitForSeconds(flare.FireInterval);
            flare.Fire();
        }
    }
}
