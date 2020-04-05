using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsItem : MonoBehaviour
{
    public int time;
    void Start()
    {
        StartCoroutine(Life());
    }

    IEnumerator Life()
    {
        int t = time;
        while (true)
        {
            if (t <= 0)
            {
                Destroy(this.gameObject);
                break;
            }
            t--;
            yield return new WaitForSeconds(1f);
        }
    }
}
