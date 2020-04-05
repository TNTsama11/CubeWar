using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStoped : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        Destroy(this.gameObject);
    }
}
