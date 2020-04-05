using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEndEvent : MonoBehaviour
{
    public void StopDamage()
    {
        this.GetComponent<Animator>().SetBool("Damage", false);
    }
}
