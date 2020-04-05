using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnCountDown : UIBase
{

    private Text countDownText;

	void Awake()
	{
        Bind(UIEvent.UI_SET_COUNTDOWN);
        countDownText = this.GetComponent<Text>();
        this.gameObject.SetActive(false);
	}

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_SET_COUNTDOWN:
                SetText(message.ToString());
                break;
            default:
                break;
        }
    }

    private void SetText(string text)
    {
        countDownText.text = text;
        this.gameObject.SetActive(true);
        if (text=="0")
        {
            this.gameObject.SetActive(false);
        }
    }
}
