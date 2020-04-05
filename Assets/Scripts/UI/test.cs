using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test:UIBase
{
    [SerializeField]
    private Button btn;


	void Start ()
    {
        btn.onClick.AddListener(onClick);
	}
	
	void Update ()
    {
		
	}

    void onClick()
    {
        Dispatch(AreaCode.TRANSFORM, TransformEvent.TRANS_MOVE, "一步两步");
        
    }
}
