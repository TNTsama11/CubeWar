using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer 
{
    public bool flag = true;

	public static IEnumerator MTimer(float time,Action act)
    {
            yield return new WaitForSeconds(time);
            act.Invoke();

    }

    public  IEnumerator MTimerLoop(float time, Action act)
    {
        while (flag)
        {
            yield return new WaitForSeconds(time);
            act.Invoke();
        }
    }

}
