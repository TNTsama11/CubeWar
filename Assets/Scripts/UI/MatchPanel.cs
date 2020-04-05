using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanel : UIBase
{
    private Image MatchImg;
    private Text MatchText;
   // private Button EnterBtn;
    private Button ExitBtn;
    private Button ReadyBtn;
    private SocketMessage smg;
    private GameObject MatchRoomPanel;
    /// <summary>
    /// 存储玩家信息面板
    /// </summary>
    private Stack<Image> matchWaitPlayerImgStack = new Stack<Image>();
    /// <summary>
    /// 已经加入的玩家信息面板
    /// </summary>
    private Dictionary<string, Image> matchOnWaitPlayerImgDict = new Dictionary<string, Image>();
	void Awake()
	{
        Bind(
            UIEvent.UI_SHOW_MATCH_MENU, UIEvent.UI_HIDE_MATCH_MENU,UIEvent.UI_START_MATCH,
            MatchEvent.MATCH_ADD_PLAYER,MatchEvent.MATCH_REMOVE_PLAYER,
            MatchEvent.MATCH_READY_FALSE,MatchEvent.MATCH_READY_TRUE,
            MatchEvent.MATCH_SHOW_PANEL
            );

        MatchImg = this.transform.Find("MatchImg").GetComponent<Image>();
        MatchText = this.transform.Find("MatchText").GetComponent<Text>();
        //EnterBtn = this.transform.Find("EnterBtn").GetComponent<Button>();
        ExitBtn = this.transform.Find("ExitBtn").GetComponent<Button>();
        ReadyBtn = this.transform.Find("ReadyBtn").GetComponent<Button>();
        MatchRoomPanel = this.transform.Find("MatchRoomPanel").gameObject;
        for(int i = 0; i <= 7; i++)
        {
            Image img = MatchRoomPanel.transform.Find("MatchIcon" + i).GetComponent<Image>();
            matchWaitPlayerImgStack.Push(img);
            img.gameObject.SetActive(false);
        }
    }
	
	void Start () 
	{
        smg = new SocketMessage();
        ExitBtn.onClick.AddListener(Exit);
        ReadyBtn.onClick.AddListener(Ready);
        // EnterBtn.onClick.AddListener(Enter);
        SetWaitPanelActive(false);
        MatchSetActive(false);
        //EnterBtn.gameObject.SetActive(false);
    }
    float timer = 0f;
    float intervalTime = 0.5f;
	void Update () 
	{
        if (MatchText.gameObject.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer >= intervalTime)
            {
                MatchTextAnime();
                timer = 0;
            }
        }
		
	}

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_HIDE_MATCH_MENU:
                MatchSetActive((bool)message);
                //ShowEnterBtn(false);
                break;
            case UIEvent.UI_SHOW_MATCH_MENU:
                MatchSetActive((bool)message);                
                break;
            case UIEvent.UI_START_MATCH:
                StartMatch();
                break;
            case MatchEvent.MATCH_ADD_PLAYER:
                //显示接收到的玩家信息面板
                AddWaitPlayer(message as UserDto);
                break;
            case MatchEvent.MATCH_REMOVE_PLAYER:
                //移除离开的玩家信息面板
                RemoveWaitPlayer(message.ToString());
                break;
            case MatchEvent.MATCH_READY_TRUE:
                //显示玩家已经准备
                SetWaitPlayerReady(message.ToString(), true);
                break;
            case MatchEvent.MATCH_READY_FALSE:
                //显示玩家未准备
                SetWaitPlayerReady(message.ToString(), false);
                break;
            case MatchEvent.MATCH_SHOW_PANEL:
                //进入房间后显示面板
                SetWaitPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private void StartMatch()
    {
        Debug.Log("开始匹配");
        //发送匹配请求
        //smg.Change(OpCode.MATCH, MatchCode.MATCH_ENTER_CREQ, null);
        //发起进入游戏请求看是否有空余游戏房间，没有就发起匹配请求
        smg.Change(OpCode.GAME, GameCode.GAME_ENTER_CERQ, null);
        Dispatch(AreaCode.NET, 0, smg);
    }

    private void MatchSetActive(bool active)
    {
        MatchImg.gameObject.SetActive(active);
        MatchText.gameObject.SetActive(active);
        ExitBtn.gameObject.SetActive(active);
        ReadyBtn.gameObject.SetActive(active);
    }

    //private void ShowEnterBtn(bool active)
    //{
    //    EnterBtn.gameObject.SetActive(active);
    //}

    //private void Enter()
    //{
    //    //TODO
    //    //像服务器发进入的请求
    //}

    private void Exit()
    {
        
        foreach(var item in matchOnWaitPlayerImgDict.Keys)
        {
            Image img = matchOnWaitPlayerImgDict[item];
            matchWaitPlayerImgStack.Push(img);
            img.gameObject.SetActive(false);
        }
        matchOnWaitPlayerImgDict.Clear();
        SetWaitPanelActive(false);
        MatchSetActive(false);
        Dispatch(AreaCode.UI, UIEvent.UI_SHOW_MAIN_MENU, true);
        //向服务发离开匹配的请求
        smg.Change(OpCode.MATCH, MatchCode.MATCH_EXIT_CREQ, null);
        Dispatch(AreaCode.NET, 0, smg);
        smg.Change(OpCode.USER, UserCode.USER_OFFLINE_CREQ, null);
        Dispatch(AreaCode.NET, 0, smg);
        Dispatch(AreaCode.UI, UIEvent.UI_SHOW_MODEL_VIEW, true);
    }

   

    private string text = "正在等待玩家";
    private int dot = 0;
    private void MatchTextAnime()
    {
        MatchText.text = text;
        dot++;
        if (dot > 6)
        {
            dot = 0;
        }
        for(int i = 0; i < dot; i++)
        {
            MatchText.text += ".";
        }
    }

   
    private void SetWaitPanelActive(bool active)
    {
        MatchRoomPanel.SetActive(active);
    }
    
    /// <summary>
    /// 添加在等待的玩家信息面板
    /// </summary>
    private void AddWaitPlayer(UserDto dto)
    {
        if (matchWaitPlayerImgStack.Count > 0)
        {
            Image img = matchWaitPlayerImgStack.Pop();
            Sprite sprite = Resources.Load("Icon/Icon" + dto.IconID, typeof(Sprite)) as Sprite;
            img.sprite = sprite;
            img.transform.Find("NameText").GetComponent<Text>().text = dto.Name;
            //TODO
            //点击显示玩家信息
            img.gameObject.SetActive(true);
            matchOnWaitPlayerImgDict.Add(dto.Account, img);
        }
        
    }
    /// <summary>
    /// 移除退出等待的的玩家
    /// </summary>
    /// <param name="acc"></param>
    private void RemoveWaitPlayer(string acc)
    {
        Debug.Log("执行RemoveWaitPlayer");
        Image img = matchOnWaitPlayerImgDict[acc];
        matchWaitPlayerImgStack.Push(img);
        matchOnWaitPlayerImgDict.Remove(acc);
        img.gameObject.SetActive(false);
    }
    /// <summary>
    /// 设置等待房间内玩家的准备状态
    /// </summary>
    private void SetWaitPlayerReady( string acc,bool active)
    {
        Image img = matchOnWaitPlayerImgDict[acc];
        img.transform.Find("ReadyText").gameObject.SetActive(active);
    }
    private bool isReady = false;
    /// <summary>
    /// 准备
    /// </summary>
    private void Ready()
    {
        string acc = PlayerPrefs.GetString("ID");
        Image img = matchOnWaitPlayerImgDict[acc];
        if (!isReady)
        {
            isReady = true;
            smg.Change(OpCode.MATCH, MatchCode.MATCH_READY_CREQ, null);
            Dispatch(AreaCode.NET,0,smg);
            img.transform.Find("ReadyText").gameObject.SetActive(true);
            ReadyBtn.transform.Find("Text").GetComponent<Text>().text = "取消准备";

        }
        else
        {
            ReadyBtn.transform.Find("Text").GetComponent<Text>().text = "准备";
            smg.Change(OpCode.MATCH, MatchCode.MATCH_NOTREADY_CREQ, null);
            Dispatch(AreaCode.NET, 0, smg);
            img.transform.Find("ReadyText").gameObject.SetActive(false);
            isReady = false;
        }
        
    }

}
 