using CommunicationProtocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : UIBase
{
    [SerializeField]
    private Button btn;
    public bool isConnected = false;
    public GameObject createAccountPanel;
    private SocketMessage socketMessage = new SocketMessage();

    void Awake()
	{
         Bind(UIEvent.UI_LOGIN_NOACC);
        btn.onClick.AddListener(onClick);
	}
	

    private void onClick()
    {
        if (isConnected)
        {
            string acc = PlayerPrefs.GetString("ID", "0");
            socketMessage.Change(OpCode.ACCOUNT, AccCode.ACC_LOGIN_CREQ, acc);
            Dispatch(AreaCode.NET, 0, socketMessage);            
        }
        else
        {
            ShowToast.MakeToast("无法接到服务器");
        }
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_LOGIN_NOACC:
                createAccountPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

}
