using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextAnim : MonoBehaviour
{
    
    void Awake()
    {
        
    }

    void Start()
    {
        this.GetComponent<RectTransform>().DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        
    }
}
