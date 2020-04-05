using CommunicationProtocol.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class IDInputCheck : UIBase
{
    public InputField inputField;
    private PanelAnimation myAnimation;
    public Text infoText;
    public Button enterBtn;
    public Button closeBtn;
    private string error1 = "<color=red>※请输入正确的ID</color>";
    private string error2 = "<color=red>※未找到该ID对应的数据，请检查ID是否正确或重新创建角色</color>";

    void Awake()
    {
        //Bind();
    }

    void Start()
    {
        infoText.text = string.Empty;
        inputField.onValueChanged.AddListener(OnInputChanged);
        enterBtn.onClick.AddListener(OnEnterBtnClick);
        enterBtn.interactable = false;
        closeBtn.onClick.AddListener(OnCloseBtnClick);
        myAnimation = this.GetComponent<PanelAnimation>();
    }

    private void OnCloseBtnClick()
    {
        inputField.text = string.Empty;
        infoText.text = string.Empty;
        myAnimation.ShowHide();
    }

    private void OnEnterBtnClick()
    {
        closeBtn.interactable = false;
        //TODO 向服务器发送引继请求
        SocketMessage socketMessage = new SocketMessage(OpCode.ACCOUNT,AccCode.ACC_RELOAD_CREQ, inputField.text.Trim());
        Dispatch(AreaCode.NET, 0, socketMessage);
    }

    private void OnInputChanged(string arg0)
    {
        if(!Regex.IsMatch(arg0, @"^[a-zA-Z0-9]{32}$"))
        {
            ShowError1();
        }
        else
        {
            ResetText();
            enterBtn.interactable = true;
        }
    }

    void Update()
    {
        
    }

    private void ShowError1()
    {
        infoText.text = error1;
    }
    private void ShowError2()
    {
        infoText.text = error2;
    }
    private void ResetText()
    {
        infoText.text = string.Empty;
    }


}
