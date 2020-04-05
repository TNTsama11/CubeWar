using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameInfoPanel : UIBase
{
    private List<string> messageList = new List<string>(5);
    private Text HPText;
    private Text HGText;
    private Text KillText;
    private Text InfoText;
    private Button MenuBtn;
    private GameObject MenuPanel;
    private GameObject MenuShowPos;
    private GameObject MenuBtnShowPos;
    private MyTimer myTimer;

    private void Awake()
    {
        Bind(UIEvent.UI_SET_GAME_HP,UIEvent.UI_SET_GAME_HG,
            UIEvent.UI_SET_GAME_KILL,UIEvent.UI_SET_GAME_INFO);

        HPText = this.transform.Find("PlayerInfolPanel").transform.Find("HPText").GetComponent<Text>();
        HGText = this.transform.Find("PlayerInfolPanel").transform.Find("HGText").GetComponent<Text>();
        KillText = this.transform.Find("PlayerInfolPanel").transform.Find("KillText").GetComponent<Text>();
        InfoText = this.transform.Find("InfoPanel").transform.Find("InfoText").GetComponent<Text>();
        MenuBtn = this.transform.Find("MenuBtn").GetComponent<Button>();
        MenuPanel = this.transform.Find("MenuPanel").gameObject;
        MenuShowPos = this.transform.Find("MenuShowPos").gameObject;
        MenuBtnShowPos= this.transform.Find("MenuBtnShowPos").gameObject;
        myTimer = new MyTimer();
    }
    private void Start()
    {
        MenuBtn.onClick.AddListener(ShowMenu);
        
    }
    private float tim=10f;
    private void Update()
    {
        if (tim > 0)
        {
            tim -= Time.deltaTime;
        }
        else
        {
            DeleteInfoText();
            tim = 10f;
        }
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_SET_GAME_HP:
                SetHp((int)message);
                break;
            case UIEvent.UI_SET_GAME_HG:
                SetHg((int)message);
                break;
            case UIEvent.UI_SET_GAME_KILL:
                SetKill((int)message);
                break;
            case UIEvent.UI_SET_GAME_INFO:
                SetInfo(message.ToString());
                break;
            default:
                break;
        }
    }

    private void SetHp(int hp)
    {
        HPText.text = hp.ToString();
    }

    private void SetHg(int hg)
    {
        HGText.text = hg.ToString();
    }

    private void SetKill(int kill)
    {
        KillText.text = kill.ToString();
    }

    private void SetInfo(string message)
    {
        if (messageList.Count < 5)
        {
            messageList.Add(message);
        }
        else
        {
            messageList.RemoveAt(0);
            messageList.Add(message);
        }
        RefreshInfoText();        
    }

    private void DeleteInfoText()
    {
        if (messageList.Count == 0)
        {
            return;
        }
        messageList.RemoveAt(0);
        RefreshInfoText();
    }

    private void RefreshInfoText()
    {
        InfoText.text = string.Empty;
        foreach (var msg in messageList)
        {
            InfoText.text += msg;
        }
    }

    private void ShowMenu()
    {
        if (!MenuPanel.activeSelf)
        {
            MenuPanel.SetActive(true);
            MenuPanel.transform.DOLocalMove(MenuShowPos.transform.localPosition, 0.5f);
            MenuBtn.transform.DOLocalRotate(MenuBtnShowPos.transform.localRotation.eulerAngles, 0.5f);
            return;
        }
    }

}
