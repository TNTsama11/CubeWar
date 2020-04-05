using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickCtrl : UIBase
{
    [SerializeField]
    private ETCJoystick joystick;
    public GameObject easyTouchCanvas;

    private void Awake()
    {
        Bind(UIEvent.UI_SHOWHIDE_ETC);
        joystick = GetComponent<ETCJoystick>();
        joystick.onMove.AddListener(onMove);
        joystick.onMoveEnd.AddListener(onMoveEnd);
    }

    void Start ()
    {

	}
	
	void Update ()
    {
		
	}

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_SHOWHIDE_ETC:
                SetETCActive((bool)message);
                break;
            default:
                break;
        }
    }

    void onMove(Vector2 direction)
    {
        Dispatch(AreaCode.TRANSFORM,TransformEvent.TRANS_MOVE,direction);        
        //Dispatch(AreaCode.AUDIO, AudioEvent.AUDIO_PLAY, "走路声");
         
    }
    void onMoveEnd()
    {
       // Dispatch(AreaCode.AUDIO, AudioEvent.AUDIO_STOP, "走路声停止");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        joystick.onMove.RemoveListener(onMove);
    }

    private void SetETCActive(bool active)
    {
        //easyTouchCanvas.SetActive(active);
        joystick.activated = active;
    }
}
