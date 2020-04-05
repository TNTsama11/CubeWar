using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayAudio : MonoBehaviour
{

    public AudioClip clickClip;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(clickClip, this.transform.position, 1f);
        }
#elif UNITY_ANDROID
        if (Input.touches[0].phase == TouchPhase.Began )
        {
            AudioSource.PlayClipAtPoint(clickClip,this.transform.position,1f);
        }
#endif
    }
}
