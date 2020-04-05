using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelAnimation : MonoBehaviour
{
    public Transform outPos;
    public Transform inPos;
    private bool isShow = false;
    

    public void ShowHide()
    {
        if (isShow)
        {
            this.transform.DOMove(outPos.position, 0.2f).SetEase(Ease.InFlash).OnComplete(()=> { isShow = false; });
            
        }
        else
        {
            this.transform.DOMove(inPos.position, 0.2f).SetEase(Ease.InFlash).OnComplete(() => { isShow = true; });
        }
    }
}
