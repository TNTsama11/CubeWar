using CommunicationProtocol.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountPanel : UIBase
{
    public Button createBtn;
    public Button reloadBtn;
    public PanelAnimation reloadPanel;
    
    void Awake()
    {
        
    }

    void Start()
    {
        createBtn.onClick.AddListener(OnCreateBtnClick);
        reloadBtn.onClick.AddListener(OnReloadBtnClick);
    }

    private void OnReloadBtnClick()
    {
        reloadPanel.ShowHide();        
    }

    private void OnCreateBtnClick()
    {
        //TODO 向服务器发送创建请求
        SocketMessage socketMessage = new SocketMessage(OpCode.ACCOUNT,AccCode.ACC_CREATE_CREQ,null);
        Dispatch(AreaCode.NET, 0, socketMessage);
    }

    void Update()
    {
        
    }
}
